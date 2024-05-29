import os
import tensorflow as tf

# Directory settings
INPUT_DIR = os.path.join('data')
DATASET_DIR = os.path.join(INPUT_DIR, 'gldv2_micro')
TEST_IMAGE_DIR = os.path.join(DATASET_DIR, 'upload')
TRAIN_IMAGE_DIR = os.path.join(DATASET_DIR, 'images')
TRAIN_LABELMAP_PATH = os.path.join(DATASET_DIR, 'gldv2_micro.csv')
LANDMARK_NAMES_PATH = 'data/train_label_to_landmark.csv'

# Retrieval
NUM_TO_RERANK = 3


# RANSAC parameters
MAX_INLIER_SCORE = 35
MAX_REPROJECTION_ERROR = 6.0
MAX_RANSAC_ITERATIONS = 10_000_000
HOMOGRAPHY_CONFIDENCE = 0.99

# DELG model settings
SAVED_MODEL_DIR = 'data/DELG_model/local_and_global'
DELG_IMAGE_SCALES = [0.70710677, 1.0, 1.4142135]
DELG_SCORE_THRESHOLD = 175.

# Feature extraction settings
LOCAL_FEATURE_NUM = tf.constant(1000)


# Load DELG model and configure tensor inputs
def load_delg_model():
    model = tf.saved_model.load(SAVED_MODEL_DIR)
    image_scales_tensor = tf.convert_to_tensor(DELG_IMAGE_SCALES)
    score_threshold_tensor = tf.constant(DELG_SCORE_THRESHOLD)
    input_tensor_names = ['input_image:0', 'input_scales:0', 'input_abs_thres:0']

    global_feature_extraction_fn = model.prune(
        input_tensor_names, ['global_descriptors:0'])

    local_feature_extraction_fn = model.prune(
        input_tensor_names + ['input_max_feature_num:0'],
        ['boxes:0', 'features:0'])

    return {
        "global_features": global_feature_extraction_fn,
        "local_features": local_feature_extraction_fn,
        "image_scales_tensor": image_scales_tensor,
        "score_threshold_tensor": score_threshold_tensor
    }


delg_model_data = load_delg_model()

global_feature_extraction_fn = delg_model_data['global_features']
local_feature_extraction_fn = delg_model_data['local_features']
image_scales_tensor = delg_model_data['image_scales_tensor']
score_threshold_tensor = delg_model_data['score_threshold_tensor']