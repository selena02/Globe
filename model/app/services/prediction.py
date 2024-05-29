import numpy as np
from scipy import spatial
from ..config import config
from . import feature_extraction
import json
from . import ransac


def predict(labelmap):
    train_ids, train_embeddings = load_training_data()
    test_id, test_embedding = feature_extraction.global_features(config.TEST_IMAGE_DIR)
    nearest = find_nearest_neighbors(test_embedding, train_embeddings, train_ids)
    scores_labels = [(train_id, labelmap[train_id], 1. - cosine_distance) for train_id, cosine_distance in nearest]
    rescored_labels_and_scores = ransac.rerank_with_inliers(test_id, scores_labels)
    prediction = prediction_map(rescored_labels_and_scores)

    return prediction


def load_training_data():
    with open('data/global_features/ids.json', 'r') as file:
        train_ids = json.load(file)
    train_embeddings = np.load('data/global_features/features.npy').tolist()

    return train_ids, train_embeddings


def find_nearest_neighbors(test_embedding, train_embeddings, train_ids):
    distances = spatial.distance.cdist(test_embedding, train_embeddings, 'cosine')[0]
    partition = np.argpartition(distances, config.NUM_TO_RERANK)[:config.NUM_TO_RERANK]

    return sorted([(train_ids[i], distances[i]) for i in partition], key=lambda x: x[1])


def prediction_map(scores_labels):
    aggregate_scores = {label: 0 for _, label, _ in scores_labels}
    for _, label, score in scores_labels:
        aggregate_scores[label] += score
    if aggregate_scores:
        label, score = max(aggregate_scores.items(), key=lambda x: x[1])
        return {'name': label, 'score': score, 'error': None}

    return {'name': None, 'score': None, 'error': None}
