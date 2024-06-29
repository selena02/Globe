import json
import os
import csv
from PIL import Image
import tensorflow as tf
import numpy as np


def get_image_path(dataset_dir, subset, image_id):
    """Generate a file path for an image that can be in png, jpg, or jpeg format."""
    extensions = ['jpg', 'jpeg', 'png']

    for ext in extensions:
        file_path = os.path.join(dataset_dir, subset, f'{image_id}.{ext}')
        if os.path.exists(file_path):
            return file_path

    return None


def load_image_tensor(image_path):
    """Load an image from a file path into a TensorFlow tensor."""
    with Image.open(image_path) as img:
        return tf.convert_to_tensor(np.array(img.convert('RGB')))


def load_labelmap(labelmap_path):
    """Load label map from a CSV file."""
    with open(labelmap_path, mode='r') as csv_file:
        csv_reader = csv.DictReader(csv_file)
        return {row['filename'].rstrip('.jpg'): row['landmark_id'] for row in csv_reader}


def load_landmark_name(filepath):
    with open(filepath, mode='r', encoding='utf-8') as csv_file:
        csv_reader = csv.DictReader(csv_file)
        return {row['landmark_id']: row['name'] for row in csv_reader}


