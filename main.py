#!/usr/bin/env python
# coding: utf-8

# In[ ]:


from google.colab import drive
drive.mount('/content/drive')


# In[ ]:


#!unzip "/content/drive/MyDrive/dataset zip/Train_7Classes.zip" -d "/content"
#!unzip "/content/drive/MyDrive/dataset zip/Train_7Classes_reduced.zip" -d "/content"
#!unzip "/content/drive/MyDrive/Train_7Classes_reduced.zip" -d "/content"
#!unzip "/content/drive/MyDrive/dataset_8classesnew.zip" -d "/content"
get_ipython().system('unzip "/content/drive/MyDrive/dataset_8classesnew_split.zip" -d "/content"')


# In[ ]:


##### TESTS PCGT


# In[ ]:


from keras.preprocessing.image import ImageDataGenerator
from keras.applications import InceptionV3
from tensorflow.keras.applications.inception_v3 import preprocess_input

from keras.applications.xception import Xception
from keras.applications.xception import preprocess_input

from keras.applications.vgg16 import VGG16
from keras.applications.vgg16 import preprocess_input


from tensorflow.keras.applications.inception_resnet_v2 import InceptionResNetV2
from tensorflow.keras.applications.inception_resnet_v2 import preprocess_input

from keras.layers import Dense, GlobalAveragePooling2D
from keras.models import Model
from tensorflow.keras.optimizers import Adam


# Define the input size of the images expected by InceptionV3
w = 299
h = 299
# Define the number of training samples, validation samples, number of epochs, and batch size

epochs = 60
batch_size = 32

# Define the path to the directory that contains the training and validation data
train_data_dir = 'train/'
validation_data_dir = 'validation/'

# Create a data generator for augmenting and normalizing the training data
from keras.preprocessing.image import ImageDataGenerator

# define data generator
datagen = ImageDataGenerator(
    preprocessing_function=preprocess_input
    #rescale=1./255,
    #validation_split=0.2
    )


# create generator for training data
X_train = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Train",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='training'
    )

# create generator for validation data
X_Val = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Validation",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='validation'
    )

X_test = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Test",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='test'
    )




# Load the pre-trained InceptionV3 model
base_model = VGG16(weights='imagenet', include_top=False)

# Add a global spatial average pooling layer
x = base_model.output
x = GlobalAveragePooling2D()(x)

# Add a fully connected layer with 1024 units and ReLU activation
x = Dense(1024, activation='relu')(x)
x = Dense(512, activation='relu')(x)

# Add a final fully connected layer with as many units as there are classes and a softmax activation
predictions = Dense(len(X_train.class_indices), activation='softmax')(x)

# Combine the base model and the final layers
model = Model(inputs=base_model.input, outputs=predictions)

# Freeze all layers in the base model
for layer in base_model.layers:
    layer.trainable = False

# Compile the model with the Adam optimizer
#model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
model.compile(optimizer=Adam(learning_rate=1e-4), loss='categorical_crossentropy', metrics=['accuracy'])


# Fine-tune the model on the new data
model.fit_generator(
        X_train,
        steps_per_epoch=X_train.samples // batch_size,
        epochs=epochs,
        validation_data=X_Val,
        validation_steps=X_Val.samples // batch_size)

# Save the model
model.save('fine_tuned_VGG16.h5')


# In[ ]:


import numpy as np


y_test_new = X_test.classes

pred = np.argmax(model.predict(X_test), axis=-1)

#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)

from sklearn.metrics import confusion_matrix,classification_report
labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                      'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                      'Speed limit (100km/h)', 'Speed limit (120km/h)'])
print(classification_report(y_test_new, pred, target_names=labels_ConfusionM))


# In[ ]:


from keras.preprocessing.image import ImageDataGenerator


w = 224
h = 224

# define data generator
datagen = ImageDataGenerator(
    #rescale=1./255,
    #validation_split=0.2
    )


# create generator for training data
X_train = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Train",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='training'
    )

# create generator for validation data
X_Val = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Validation",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='validation'
    )

X_test = datagen.flow_from_directory(
    "/content/content/dataset_8classesnew_split_new2/Test",
    target_size=(w, h),
    batch_size=32,
    color_mode='rgb',
    class_mode='categorical',
    shuffle=False,
    #subset='test'
    )



# In[ ]:


import tensorflow as tf
from keras.applications.inception_v3 import InceptionV3
from keras.applications.vgg16 import VGG16
from keras.applications.vgg19 import VGG19
from keras.applications.xception import Xception
from tensorflow.keras.applications.resnet50 import ResNet50
from tensorflow.keras.applications import EfficientNetB0
from tensorflow.keras.applications.inception_resnet_v2 import InceptionResNetV2
from keras.applications.mobilenet import MobileNet

from keras.preprocessing import image
from keras.models import Model
from keras.layers import Dense, GlobalAveragePooling2D
from keras.layers import Input

# create the base pre-trained model
input_tensor = Input(shape=(w, h, 3))


########################### VGG16 ###########################
base_model = VGG16(input_tensor=input_tensor, weights='imagenet', include_top=False)
#base_model = VGG19(input_tensor=input_tensor, weights='imagenet', include_top=False)
#base_model = Xception(input_tensor=input_tensor, weights='imagenet', include_top=False)

# add a global spatial average pooling layer
x = base_model.output
x = GlobalAveragePooling2D()(x)
# let's add a fully-connected layer
#x = Dense(256, activation='relu')(x)

# and a logistic layer
predictions = Dense(8, activation='softmax')(x)


# This callback will stop the training when there is no improvement in
# the loss for three consecutive epochs.
callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)

# this is the model we will train
model = Model(inputs=base_model.input, outputs=predictions)

# first: train only the top layers (which were randomly initialized)
# i.e. freeze all convolutional InceptionV3 layers
for layer in base_model.layers:
    layer.trainable = False


# compile the model (should be done *after* setting layers to non-trainable)
model.compile(optimizer='Adam', loss='categorical_crossentropy')
# train the model on the new data for a few epochs
#model.fit(X_train, y_train, validation_data=(X_Val, y_Val), batch_size=32, epochs=20, callbacks=[callback])
model.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=20)


########################### VGG-16 -> 7th   ###########################

for layer in model.layers[:7]:
   layer.trainable = False
for layer in model.layers[7:]:
   layer.trainable = True


# we need to recompile the model for these modifications to take effect
# we use Adam with a low learning rate
from tensorflow.keras.optimizers import Adam

model.summary()
model.compile(optimizer=Adam(learning_rate=1e-5), loss='categorical_crossentropy', metrics=['accuracy'])

callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)
# we train our model again (this time fine-tuning the top 2 inception blocks
# alongside the top Dense layers
#model.fit(X_train, y_train, validation_data=(X_Val, y_Val), batch_size=32, epochs=10, callbacks=[callback])
model.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=10)


# In[ ]:


model.save('VGG16_Extended8.h5')


# In[ ]:


import numpy as np


y_test_new = X_test.classes

pred = np.argmax(model.predict(X_test), axis=-1)

#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)

from sklearn.metrics import confusion_matrix,classification_report
labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                      'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                      'Speed limit (100km/h)', 'Speed limit (120km/h)'])
print(classification_report(y_test_new, pred, target_names=labels_ConfusionM))


# In[ ]:


# create the base pre-trained model
input_tensor = Input(shape=(w, h, 3))

########################### ResNet50 ###########################
base_model2 = ResNet50(input_tensor=input_tensor, weights='imagenet', include_top=False)


# add a global spatial average pooling layer
x = base_model2.output
x = GlobalAveragePooling2D()(x)
# let's add a fully-connected layer
#x = Dropout(0.3)(x)

# and a logistic layer
predictions = Dense(8, activation='softmax')(x)


# This callback will stop the training when there is no improvement in
# the loss for three consecutive epochs.
callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)

# this is the model we will train
model2 = Model(inputs=base_model2.input, outputs=predictions)

# first: train only the top layers (which were randomly initialized)
# i.e. freeze all convolutional InceptionV3 layers
for layer in base_model2.layers:
    layer.trainable = False


# compile the model (should be done *after* setting layers to non-trainable)
model2.compile(optimizer='Adam', loss='categorical_crossentropy')
# train the model on the new data for a few epochs
model2.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=40)

########################### RESNET-50-> 81st  ###########################
for layer in model2.layers[:81]:
   layer.trainable = False
for layer in model2.layers[81:]:
   layer.trainable = True


# we need to recompile the model for these modifications to take effect
# we use Adam with a low learning rate
from tensorflow.keras.optimizers import Adam

model2.summary()
model2.compile(optimizer=Adam(learning_rate=1e-5), loss='categorical_crossentropy', metrics=['accuracy'])


callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)
# we train our model again (this time fine-tuning the top 2 inception blocks
# alongside the top Dense layers
model2.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=20)


# In[ ]:


model.save('ResNet50_Extended8.h5')


# In[ ]:


import numpy as np


y_test_new = X_test.classes

pred = np.argmax(model2.predict(X_test), axis=-1)

#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)

from sklearn.metrics import confusion_matrix,classification_report
labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                      'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                      'Speed limit (100km/h)', 'Speed limit (120km/h)'])
print(classification_report(y_test_new, pred, target_names=labels_ConfusionM))


# In[ ]:


# create the base pre-trained model
input_tensor = Input(shape=(w, h, 3))


########################### EfficientNetB0 ###########################
base_model3 = EfficientNetB0(input_tensor=input_tensor, weights='imagenet', include_top=False)



# add a global spatial average pooling layer
x = base_model3.output
x = GlobalAveragePooling2D()(x)
# let's add a fully-connected layer

########################### EfficientNetB0 ###########################
x = Dense(256, activation='relu')(x)



# and a logistic layer
predictions = Dense(8, activation='softmax')(x)


# This callback will stop the training when there is no improvement in
# the loss for three consecutive epochs.
callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)

# this is the model we will train
model3 = Model(inputs=base_model3.input, outputs=predictions)

# first: train only the top layers (which were randomly initialized)
# i.e. freeze all convolutional InceptionV3 layers
for layer in base_model3.layers:
    layer.trainable = False


# compile the model (should be done *after* setting layers to non-trainable)
model3.compile(optimizer='Adam', loss='categorical_crossentropy')
# train the model on the new data for a few epochs
model3.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=40)


# we chose to train the top 2 inception blocks, i.e. we will freeze
# the first 249 layers and unfreeze the rest:

########################### EfficientNetB0   ###########################

for layer in model3.layers[:119]:
   layer.trainable = False
for layer in model3.layers[119:]:
   layer.trainable = True


# we need to recompile the model for these modifications to take effect
# we use Adam with a low learning rate
from tensorflow.keras.optimizers import Adam

model3.summary()
model3.compile(optimizer=Adam(learning_rate=1e-5), loss='categorical_crossentropy', metrics=['accuracy'])


callback = tf.keras.callbacks.EarlyStopping(monitor='loss', patience=3)
# we train our model again (this time fine-tuning the top 2 inception blocks
# alongside the top Dense layers

model3.fit(
    X_train,
    steps_per_epoch=X_train.samples // X_train.batch_size,
    validation_data=X_Val,
    validation_steps=X_Val.samples // X_Val.batch_size,
    epochs=40)


# In[ ]:


model.save('EfficientNetB0.h5')


# In[ ]:


import numpy as np


y_test_new = X_test.classes

pred = np.argmax(model3.predict(X_test), axis=-1)

#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)

from sklearn.metrics import confusion_matrix,classification_report
labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                      'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                      'Speed limit (100km/h)', 'Speed limit (120km/h)'])
print(classification_report(y_test_new, pred, target_names=labels_ConfusionM))


# In[ ]:


# O'Hagan OWA Weights
  #Optimistic -> alpha = 0.8
import numpy as np
n = 3 #number of sources -> CNN Models
orness_old = 0.7 #Pessimistic
alpha = 0.58 #From graph
Temp = np.zeros((3,1))
counter = 0
w1 = alpha
w2 = alpha * (1 - alpha)
w3 = (1 - alpha) ** (n-1)
W_optimistic = np.array([w1, w2, w3])

for i in range(n):
  counter = counter+1
  Temp[i] = (n-i-1) * W_optimistic[i]
orness_new = (1/2)*sum(Temp)

if np.abs(orness_new - orness_old) > 0.0001:
  orness_old = orness_new
W_optimistic


# In[ ]:


# O'Hagan OWA Weights
  #Pessimistic -> alpha = 0.84
import numpy as np
n = 3 #number of sources -> CNN Models
orness_old = 0.7 #Optimistic
alpha = 0.79 #From graph
Temp = np.zeros((3,1))

w1 = alpha ** (n-1)
w2 = (1 - alpha) * (alpha ** (n-2))
w3 = (1 - alpha)
W_optimistic = np.array([w1, w2, w3])

for i in range(n):
  Temp[i] = (n-i-1) * W_optimistic[i]
orness_new = (1/2)*sum(Temp)

if np.abs(orness_new - orness_old) > 0.0001:
  orness_old = orness_new
W_optimistic


# In[ ]:


from keras.layers import *
from tensorflow import keras
from keras.models import Model
"""
#Add()
#Multiply() -> Adam(learning_rate=7e-6)
#Concatenate()
mergedOut = Multiply()([model.output,model2.output])
merged = Model([model.input, model2.input], mergedOut)
opt = keras.optimizers.Adam(learning_rate=1e-5)
merged.compile(loss='categorical_crossentropy', optimizer=opt, metrics=['accuracy'])
merged.summary()


from keras.layers import Layer, Input, Dense
from keras.models import Model
import keras.backend as K
import tensorflow as tf
from tensorflow.keras import layers

class OWA(layers.Layer):
#A custom keras layer for OWA

    def __init__(self,**kwargs):

        super(OWA,self).__init__(**kwargs)

    def build(self,input_shape):
        self.alpha = self.add_weight(
            name = 'alpha', shape=(input_shape[1]),
            initializer = 'zeros',
            trainable = True
        )
        super(OWA,self).build(input_shape)

    def call(self,LayerOutput):
        #return tf.add(LayerOutput, 1)
        return tf.sort(LayerOutput, axis=-1, direction='DESCENDING', name=None)
        #Weight = (tf.maximum(0.,X[0]))
        #return tf.keras.layers.add([X])

        #return tf.math.reduce_max(LayerOutput, axis=0)
        #return tf.maximum(0.,x)+ self.alpha * tf.minimum(0.,x)

    def compute_output_shape(self, input_shape):
        return input_shape[0]

#out = WeightedSum()([model.output, model2.output, model3.output])
"""


# In[ ]:


from keras.layers import *
from tensorflow import keras
from keras.models import Model
"""
#Add()
#Multiply() -> Adam(learning_rate=7e-6)
#Concatenate()
mergedOut = Multiply()([model.output,model2.output])
merged = Model([model.input, model2.input], mergedOut)
opt = keras.optimizers.Adam(learning_rate=1e-5)
merged.compile(loss='categorical_crossentropy', optimizer=opt, metrics=['accuracy'])
merged.summary()
"""

from keras.layers import Layer, Input, Dense
from keras.models import Model
import keras.backend as K
import tensorflow as tf
from tensorflow.keras import layers



class WeightedSum(layers.Layer):
# OWA Keras Layer

    def __init__(self, **kwargs):
        super(WeightedSum, self).__init__(**kwargs)

    def build(self, input_shape=1):

        #self.a = self.add_weight(
        #    name='alpha1',
        #    shape=(),
        #    initializer='ones',
        #    dtype='float32',
        #    trainable=True,
        #)
        #tf.keras.constraints.MinMaxNorm(min_value=0.0, max_value=1.0)

        super(WeightedSum, self).build(input_shape)


    def call(self, model_outputs):
        #return (self.a * model_outputs[0] + self.b * model_outputs[1])
        #return ((self.a/(self.a + self.b)) * model_outputs[0]) + ((self.b/(self.a + self.b)) * model_outputs[1])
        #print('//////////////',self.a)
        #print('//////////',self.a + self.b + self.c)
        return ((W_optimistic[0] * model_outputs[0]) + (W_optimistic[1] * model_outputs[1]) + (W_optimistic[2] * model_outputs[2]))

    def compute_output_shape(self, input_shape):
        return input_shape[0]

# Weighed sum of the two models' outputs with a = 0.1
out = WeightedSum()([model.output, model2.output, model3.output])

# Create the merged model
merged = Model(inputs=[model.input, model2.input, model3.input], outputs=[out])
opt = keras.optimizers.Adam(learning_rate=1e-5)
merged.compile(loss='categorical_crossentropy', optimizer=opt, metrics=['accuracy'])
merged.summary()

################################################################################


# In[ ]:


from keras.models import Model
layer_outputs = [layer.output for layer in model2.layers]
activation_model = Model(inputs=model2.input, outputs=layer_outputs)
activations = activation_model.predict(X_train2[4].reshape(1,w2,h2,1))
#activations = activation_model.predict([X_train2[4].reshape(1,w2,h2,1),X_train2[4].reshape(1,w2,h2,1)])

def display_activation(activations, col_size, row_size, act_index):
    activation = activations[act_index]
    activation_index=0
    fig, ax = plt.subplots(row_size, col_size, figsize=(row_size*2.5,col_size*1.5))
    for row in range(0,row_size):
        for col in range(0,col_size):
            ax[row][col].imshow(activation[0, :, :, activation_index], cmap='gray')
            activation_index += 1

display_activation(activations, 3, 3, 11)


# In[ ]:


#tf.keras.utils.plot_model(merged)
keras.utils.plot_model(merged, show_shapes=True)


# In[ ]:


epochs = 20
history_merged = merged.fit([X_train, X_train, X_train], y_train, validation_data=([X_Val, X_Val ,X_Val], y_Val), batch_size=32, epochs=epochs)


# In[ ]:


#plotting graphs for accuracy
plt.figure(0)
plt.plot(history_merged.history['accuracy'], label='training accuracy', color='green')
plt.plot(history_merged.history['val_accuracy'], label='val accuracy', color='red')
plt.title('Accuracy')
plt.xlabel('epochs')
plt.ylabel('accuracy')
plt.legend()
plt.savefig('Accuracy.png')
plt.show()

plt.figure(1)
plt.plot(history_merged.history['loss'], label='training loss', color='green')
plt.plot(history_merged.history['val_loss'], label='val loss', color='red')
plt.title('Loss')
plt.xlabel('epochs')
plt.ylabel('loss')
plt.legend()
plt.savefig('Loss.png')
#plt.axis([0, 30, 0, 10])
plt.show()


# In[ ]:


y_test_new = np.argmax(y_test, axis=1)

pred = np.argmax(merged.predict([X_test, X_test, X_test]), axis=-1)
#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)

from sklearn.metrics import confusion_matrix,classification_report
labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                      'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                      'Speed limit (100km/h)', 'Speed limit (120km/h)'])
print(classification_report(y_test_new, pred, target_names=labels_ConfusionM))


# In[ ]:


model.save('OWA_Model_Pessimistic.h5')


# In[ ]:


# Confusion Matrix

from sklearn.metrics import confusion_matrix
import matplotlib.pyplot as plt
import numpy as np
import itertools

confusion_mat = confusion_matrix(y_test_new,pred)

def plot_confusion_matrix(cm, classes,
                         normalize=False,
                         title='Confusion matrix',
                         cmap=plt.cm.Blues):
   """
   This function prints and plots the confusion matrix.
   Normalization can be applied by setting `normalize=True`.
   """
   plt.figure(figsize = (10, 10))
   #hosein:plt.figure->15,15
   #plt.figure(figsize = (8,8))
   plt.imshow(cm, interpolation='nearest', cmap=cmap)
   plt.title(title)
   plt.colorbar()
   tick_marks = np.arange(len(classes))
   plt.xticks(tick_marks, classes, rotation=90)
   plt.yticks(tick_marks, classes)
   if normalize:
       cm = cm.astype('float') / cm.sum(axis=1)[:, np.newaxis]
       cm = np.around(cm, decimals=2)

   thresh = cm.max() / 2.
   for i, j in itertools.product(range(cm.shape[0]), range(cm.shape[1])):
       plt.text(j, i, cm[i, j],
                horizontalalignment="center",
                color="white" if cm[i, j] > thresh else "black")
   plt.tight_layout()
   plt.ylabel('True label')
   plt.xlabel('Predicted label')

labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)',
                     'Speed limit (60km/h)', 'Speed limit (70km/h)', 'Speed limit (80km/h)',
                     'Speed limit (100km/h)', 'Speed limit (120km/h)'])


#labels_ConfusionM = (['Speed limit (20km/h)', 'Speed limit (30km/h)', 'Speed limit (50km/h)', 'Speed limit (60km/h)'])

#labels_ConfusionM = ['Speed limit (30km/h)', 'Speed limit (50km/h)']
plot_confusion_matrix(confusion_mat, labels_ConfusionM, normalize=True)
#plt.savefig('ConfusionMatrix_7Classes-SGD_7e-3_89.5%acc.png')
plt.savefig('ConfusionMatrix3.png')


# In[ ]:


# Classification Report

y_test_new = np.argmax(y_test, axis=1)

pred = np.argmax(merged.predict([X_test]), axis=-1)
#Accuracy with the test data

from sklearn.metrics import accuracy_score
from sklearn.metrics import precision_score, f1_score
print("Acuracy Score is: ", + accuracy_score(y_test_new, pred), "   Precision Score is: ", + precision_score(y_test_new, pred, average='macro'))
f1 = f1_score(y_test_new, pred,average='macro')
print('F1 score: %f' % f1)


# In[ ]:





# In[ ]:


from tensorflow.keras.layers import Input, Dense, concatenate
from tensorflow.keras.models import Model
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.applications.vgg16 import VGG16
from tensorflow.keras.applications.resnet50 import ResNet50
from tensorflow.keras.applications.efficientnet import EfficientNetB0
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow import keras

# Define the OWA layer
def owa_layer(x):
    w = [0.33, 0.33, 0.34]  # weights for the three models
    x = concatenate(x)
    x = Dense(3, activation='softmax')(x)
    x = x * w
    x = keras.backend.sum(x, axis=1, keepdims=True)
    return x

# Load the pre-trained models
vgg16 = VGG16(weights='imagenet', include_top=False, input_shape=(224, 224, 3))
resnet50 = ResNet50(weights='imagenet', include_top=False, input_shape=(224, 224, 3))
efficientnet = EfficientNetB0(weights='imagenet', include_top=False, input_shape=(224, 224, 3))

# Fine-tune the models on your data
# For example, using data augmentation and freezing some layers:
datagen = ImageDataGenerator()
batch_size = 32
num_classes = 8

train_generator = datagen.flow_from_directory(
    '/content/content/dataset_8classesnew_split_new2/Train',
    target_size=(224, 224),
    batch_size=batch_size,
    class_mode='categorical')

validation_generator = datagen.flow_from_directory(
    '/content/content/dataset_8classesnew_split_new2/Validation',
    target_size=(224, 224),
    batch_size=batch_size,
    class_mode='categorical')

for layer in vgg16.layers[:-4]:
    layer.trainable = False
for layer in resnet50.layers[:-4]:
    layer.trainable = False
for layer in efficientnet.layers[:-4]:
    layer.trainable = False

vgg16_output = vgg16.layers[-1].output
resnet50_output = resnet50.layers[-1].output
efficientnet_output = efficientnet.layers[-1].output

merged = owa_layer([vgg16_output, resnet50_output, efficientnet_output])
model = Model(inputs=[vgg16.input, resnet50.input, efficientnet.input], outputs=merged)

model.compile(optimizer=Adam(learning_rate=1e-4), loss='categorical_crossentropy', metrics=['accuracy'])

# Train the OWA model
model.fit(
    [train_generator, train_generator, train_generator],
    steps_per_epoch=len(train_generator),
    epochs=20,
    validation_data=([validation_generator, validation_generator, validation_generator]),
    validation_steps=len(validation_generator))

# Evaluate the model on the test set
test_generator = datagen.flow_from_directory(
    '/content/content/dataset_8classesnew_split_new2/Test',
    target_size=(224, 224),
    batch_size=batch_size,
    class_mode='categorical')

model.evaluate(
    [test_generator, test_generator, test_generator],
    steps=len(test_generator))


# In[ ]:




