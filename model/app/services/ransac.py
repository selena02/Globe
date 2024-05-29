import numpy as np
import cv2
from scipy.spatial import cKDTree
from ..config import config
from ..utils import utils
from . import feature_extraction


def get_total_score(num_inliers, global_score):
    local_score = min(num_inliers, config.MAX_INLIER_SCORE) / config.MAX_INLIER_SCORE
    return local_score + global_score


def get_putative_matching_keypoints(test_keypoints, test_descriptors, train_keypoints, train_descriptors,
                                    max_distance=0.9):
    train_descriptor_tree = cKDTree(train_descriptors)
    distances, matches = train_descriptor_tree.query(test_descriptors, distance_upper_bound=max_distance)

    # Filter matches where distance is within the threshold
    valid_indices = np.where(matches < len(train_keypoints))[0]
    test_matching_keypoints = test_keypoints[valid_indices]
    train_matching_keypoints = train_keypoints[matches[valid_indices]]

    return test_matching_keypoints, train_matching_keypoints


def get_num_inliers(test_keypoints, test_descriptors, train_keypoints, train_descriptors):

    test_match_kp, train_match_kp = get_putative_matching_keypoints(
        test_keypoints, test_descriptors, train_keypoints, train_descriptors)

    if len(test_match_kp) < 4:
        print("Not enough keypoints for RANSAC.")
        return 0

    try:
        H, mask = cv2.findHomography(test_match_kp, train_match_kp, cv2.RANSAC,
                                     ransacReprojThreshold=config.MAX_REPROJECTION_ERROR,
                                     confidence=config.HOMOGRAPHY_CONFIDENCE,
                                     maxIters=config.MAX_RANSAC_ITERATIONS)
        if mask is None:
            print("RANSAC did not converge.")
            return 0
        return np.sum(mask)
    except np.linalg.LinAlgError as e:
        print(f"Error computing homography: {e}")
        return 0


def rerank_with_inliers(test_image_id, scores_labels):
    test_image_path = utils.get_image_path(config.DATASET_DIR, 'upload', test_image_id)
    test_keypoints, test_descriptors = feature_extraction.local_features(test_image_path)
    for i in range(len(scores_labels)):
        train_image_id, label, initial_score = scores_labels[i]
        train_image_path = utils.get_image_path(config.DATASET_DIR, 'landmarks', train_image_id)
        train_keypoints, train_descriptors = feature_extraction.local_features(train_image_path)
        num_inliers = get_num_inliers(test_keypoints, test_descriptors, train_keypoints, train_descriptors)
        total_score = get_total_score(num_inliers, initial_score)

        scores_labels[i] = (train_image_id, label, total_score)

    scores_labels.sort(key=lambda x: x[2], reverse=True)
    return scores_labels
