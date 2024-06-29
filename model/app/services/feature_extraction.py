import tensorflow as tf
import pathlib
from ..config import config
from ..utils import utils


def global_features(image_dir):
    extensions = ['*.jpg', '*.png', '*.jpeg']
    image_path = next((p for p in extensions for p in pathlib.Path(image_dir).glob(p)), None)

    if not image_path:
        raise FileNotFoundError("No image found in the specified directory")

    image_id = image_path.stem
    image_tensor = utils.load_image_tensor(str(image_path))
    features = config.global_feature_extraction_fn(image_tensor, config.image_scales_tensor,
                                                   config.score_threshold_tensor)
    embeddings = tf.nn.l2_normalize(tf.reduce_sum(features[0], axis=0), axis=0).numpy()
    return image_id, embeddings.reshape(1, -1)


def local_features(image_path):
    image_tensor = utils.load_image_tensor(image_path)
    features = config.local_feature_extraction_fn(image_tensor, config.image_scales_tensor,
                                                  config.score_threshold_tensor, config.LOCAL_FEATURE_NUM)
    keypoints, descriptors = process_features(features)
    return keypoints, descriptors


def process_features(features):
    keypoints = tf.divide(tf.add(tf.gather(features[0], [0, 1], axis=1), tf.gather(features[0], [2, 3], axis=1)),
                          2.0).numpy()
    descriptors = tf.nn.l2_normalize(features[1], axis=1).numpy()
    return keypoints, descriptors
