# Road Sign Detection for Intelligent Transportation Systems
### CNN Ensemble with OWA Fusion in a Unity 3D Simulated Environment

[![Python](https://img.shields.io/badge/Python-3.8%2B-blue?logo=python)](https://www.python.org/)
[![TensorFlow](https://img.shields.io/badge/TensorFlow-2.x-FF6F00?logo=tensorflow)](https://www.tensorflow.org/)
[![Unity](https://img.shields.io/badge/Unity-3D-black?logo=unity)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Paper](Springer Nature)](https://doi.org/10.1007/s10489-024-06213-3

)

> **B.Sc. Final Year Project** — A road sign detection system for autonomous vehicles using a CNN ensemble fused via an Ordered Weighted Averaging (OWA) layer, evaluated in a Unity 3D traffic simulation across multiple weather and accident scenarios.

---

## Abstract

Intelligent Transportation Systems (ITS) aim to enhance road safety, and IoT-related solutions are crucial in achieving this objective. By leveraging Vehicle-to-Vehicle (V2V) and Vehicle-to-Infrastructure (V2I) technologies, drivers can access valuable information about their surroundings. This research utilised the Unity 3D game engine to simulate various traffic scenarios, exploring a stochastic environment with two data sources: camera and road sign labels. A full-duplex communication system was developed to enable communication between Python and Unity, allowing the vehicle to capture images in Unity and classify them using CNN models coded in Python. To improve road sign detection accuracy, multi-sensor Data Fusion (DF) techniques were applied using the Kalman filter, Dempster-Shafer theory, and Fuzzy Integral Operators. Furthermore, the proposed CNN model incorporates an OWA layer to fuse information from three pre-trained CNN models, achieving **98.81% accuracy** and outperforming six state-of-the-art models.

---

## Key Results

| Method | Accuracy |
|---|---|
| VGG16 (fine-tuned) | — |
| ResNet50 (fine-tuned) | — |
| EfficientNetB0 (fine-tuned) | — |
| **OWA Ensemble (VGG16 + ResNet50 + EfficientNetB0)** | **98.81%** |
| Extended Kalman Filter (EKF) execution time | 0.02 s |
| Dempster-Shafer vs Fuzzy Integral accuracy gain | ~30% better |

---

## System Overview

```
┌──────────────────────────────────────────────────────────────────┐
│                       Unity 3D Environment                       │
│  (Traffic scenes, weather conditions, accident scenarios x8)     │
└─────────────────────────┬────────────────────────────────────────┘
                          │  Full-Duplex Python ↔ Unity TCP Socket
          ┌───────────────▼───────────────┐
          │     Camera Image Stream        │
          └───────────────┬───────────────┘
                          │
          ┌───────────────▼───────────────────────────────────┐
          │              CNN Ensemble (Python)                 │
          │                                                    │
          │   ┌──────────┐ ┌──────────┐ ┌───────────────┐    │
          │   │  VGG16   │ │ ResNet50 │ │ EfficientNetB0│    │
          │   │(224×224) │ │(224×224) │ │  (224×224)    │    │
          │   └────┬─────┘ └────┬─────┘ └──────┬────────┘    │
          │        └────────────┼───────────────┘             │
          │                     │                             │
          │          ┌──────────▼──────────┐                  │
          │          │   OWA Fusion Layer   │                  │
          │          │  (O'Hagan Weights)   │                  │
          │          └──────────┬──────────┘                  │
          └─────────────────────┼──────────────────────────── ┘
                                │
          ┌─────────────────────▼──────────────────────────────┐
          │            Data Fusion (Multi-Sensor)               │
          │  ┌─────────────┐ ┌──────────────┐ ┌────────────┐  │
          │  │EKF / UKF    │ │Dempster-     │ │  Fuzzy     │  │
          │  │Kalman Filter│ │Shafer Theory │ │  Integral  │  │
          │  └─────────────┘ └──────────────┘ └────────────┘  │
          └────────────────────────────────────────────────────┘
                                │
                    ┌───────────▼───────────┐
                    │   Vehicle Decision     │
                    │  (Speed, Manoeuvre)    │
                    └───────────────────────┘
```

---

## Dataset

The model is trained on the **`Combined_Dataset (Unity_Kaggle)`** folder, which combines Unity-rendered road sign images with Kaggle road sign data. The dataset covers **8 speed limit classes**:

- Speed limit: 20 km/h, 30 km/h, 50 km/h, 60 km/h, 70 km/h, 80 km/h, 100 km/h, 120 km/h

### Folder structure expected by the code

```
Combined_Dataset (Unity_Kaggle)/
├── Train/
│   ├── Speed_20/
│   ├── Speed_30/
│   └── ... (8 classes)
├── Validation/
│   └── ... (8 classes)
└── Test/
    └── ... (8 classes)
```

---

## How It Works

### Step 1 — Individual Model Training

Running the code sequentially trains three pre-trained CNN backbones via **two-phase transfer learning**:

| Model | Input Size | Fine-tune from Layer | Epochs (Phase 1 / 2) |
|---|---|---|---|
| VGG16 | 224 × 224 | Layer 7 | 20 / 10 |
| ResNet50 | 224 × 224 | Layer 81 | 40 / 20 |
| EfficientNetB0 | 224 × 224 | Layer 119 | 40 / 40 |

Each model is saved as a `.h5` file (`VGG16_Extended8.h5`, `ResNet50_Extended8.h5`, `EfficientNetB0.h5`).

### Step 2 — OWA Ensemble Fusion

The three trained models are combined using a custom **Ordered Weighted Averaging (OWA) Keras layer** (`WeightedSum`). Weights are computed using **O'Hagan's method**:

```
Output = w1·sort(p₁) + w2·sort(p₂) + w3·sort(p₃)
```

Two weight strategies are supported:
- **Optimistic** (orness = 0.7, α = 0.58): weights `[0.58, 0.243, 0.177]`
- **Pessimistic** (orness = 0.7, α = 0.79): weights emphasise lower-confidence predictions

The final merged model is saved as `OWA_Model_Pessimistic.h5`.

### Step 3 — Data Fusion (Multi-Sensor)

Predictions from the CNN and road sign label data are fused using:
- **Extended Kalman Filter (EKF)** — faster (0.02 s), slightly less accurate
- **Unscented Kalman Filter (UKF)** — more accurate, higher computational cost
- **Dempster-Shafer Theory** — ~30% better accuracy than Fuzzy Integral
- **Fuzzy Integral Operators** — alternative evidence combination method

---

## Unity Simulation

The repository includes Unity 3D project files with the following scenes:

| Scene | Description |
|---|---|
| **Scene 1 (Main)** | Complete environment as presented in the paper — full V2V/V2I setup |
| **Scenes 2–9** | Eight additional test scenarios covering varied weather conditions and accident situations |

The Python–Unity communication uses a **full-duplex TCP socket**, allowing Unity to stream camera frames to Python in real time and receive classification results back.

---

## Installation & Usage

### Prerequisites

- Python 3.8+
- Google Colab (recommended — code is configured for Colab with Google Drive)
- Unity 2021+ (for the simulation scenes)

### Running on Google Colab

1. Upload the `Combined_Dataset (Unity_Kaggle)` folder to your Google Drive.

2. Open `main.py` in Colab and mount your Drive:
```python
from google.colab import drive
drive.mount('/content/drive')
```

3. Update the dataset paths in the data generator sections to point to your Drive location:
```python
"/content/content/dataset_8classesnew_split_new2/Train"
# → Change to your own path, e.g.:
"/content/drive/MyDrive/Combined_Dataset (Unity_Kaggle)/Train"
```

4. Run all cells sequentially. The three models are trained first, then the OWA ensemble is trained on top.

### Running Locally

```bash
git clone https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
cd YOUR_REPO_NAME
pip install -r requirements.txt
python Paper_FinalVersion2.py
```

> **Note:** Remove the `drive.mount` and `!unzip` cells if running locally.

---

## Repository Structure

```
├── README.md
├── requirements.txt
├── LICENSE
├── .gitignore
├── Paper_FinalVersion2.py          ← Main training and fusion code
├── Combined_Dataset (Unity_Kaggle)/← Dataset (Train / Validation / Test)
│   ├── Train/
│   ├── Validation/
│   └── Test/
├── Unity/                          ← Unity 3D project files
│   ├── Scene_1_Main/               ← Full paper environment
│   └── Scenes_2-9/                 ← Additional test scenarios
└── Results/                        ← Accuracy/Loss plots, Confusion matrices
```

---

## Citation

If you use this code in your research, please cite:

```bibtex
@article{norouzi2025applications,
  title={Applications of pre-trained CNN models and data fusion techniques in Unity3D for connected vehicles: M. Norouzi et al.},
  author={Norouzi, Mojtaba and Hosseini, Seyed Hossein and Khoshnevisan, Mohammad and Moshiri, Behzad},
  journal={Applied Intelligence},
  volume={55},
  number={6},
  pages={390},
  year={2025},
  publisher={Springer}
}
```

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## Contact

**Mojtaba Norouzi**  
📧 mojtaba_norouzi77@alumni.iust.ac.ir
🔗 [LinkedIn](www.linkedin.com/in/mojtaba-norouzi-2522a0206)

