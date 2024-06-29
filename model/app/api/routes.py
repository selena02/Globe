import os
from flask import Blueprint, request, jsonify
from werkzeug.utils import secure_filename
from ..services import prediction
from ..utils import utils
from ..config import config

routes = Blueprint('routes', __name__)

ALLOWED_EXTENSIONS = {'png', 'jpg', 'jpeg'}


def allowed_file(filename):
    return '.' in filename and filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS


@routes.route('/predict', methods=['POST'])
def predict_image():
    if 'image' not in request.files:
        return jsonify({'error': 'No image provided'}), 400

    image_file = request.files['image']

    if image_file.filename == '':
        return jsonify({'error': 'No file selected'}), 400

    if not allowed_file(image_file.filename):
        return jsonify({'error': 'File format not supported. Please upload a PNG or JPG file.'}), 400

    if len(request.files) > 1:
        return jsonify({'error': 'Only one image is allowed per request'}), 400

    if image_file and allowed_file(image_file.filename):
        filename = secure_filename(image_file.filename)
        image_path = os.path.join('data/gldv2_micro/upload', filename)

        try:
            image_file.save(image_path)
            if os.path.getsize(image_path) == 0:
                os.remove(image_path)
                return jsonify({'error': 'The uploaded file is empty'}), 400

            labelmap = utils.load_labelmap(config.TRAIN_LABELMAP_PATH)
            namemap = utils.load_landmark_name(config.LANDMARK_NAMES_PATH)
            result = prediction.predict(labelmap)
            if float(result['score']) < 0.7:
                return jsonify({'error': 'Failed to retrieve landmark'}), 404
            class_id = result['name']
            if class_id in namemap:
                result['name'] = namemap[class_id]
            else:
                result['name'] = "Unknown Landmark"
            os.remove(image_path)
            return jsonify(result)

        except Exception as e:
            os.remove(image_path) if os.path.exists(image_path) else None
            return jsonify({'error': 'Internal server error'}), 500
