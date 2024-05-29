FROM python:3.12-slim

WORKDIR /app

COPY requirements.txt .

RUN pip3 install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE 5000

# Set environment variables
ENV FLASK_APP=app.api:create_app
ENV FLASK_ENV=production
ENV PYTHONUNBUFFERED=1

CMD ["gunicorn", "--workers", "3", "--timeout", "120", "--bind", "0.0.0.0:5000", "app.api:create_app()"]
