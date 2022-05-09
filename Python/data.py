###Extract data from unity logged file

import csv
import os
import torch
from torch.utils.data import Dataset
import cv2

class Extract(Dataset):
    def __init__(self, filePath):
        super().__init__()
        filePath = os.path.join(filePath, 'driving_log.csv')
        self.data = self.loadCSV(filePath)


    def loadCSV(self, filePath):
        data = []

        with open(filePath,'r') as csvfile:
            reader = csv.reader(csvfile, delimiter = ',')
            for row in reader:
                data.append(row)

        return data

    def __getitem__(self, index):
        set = self.data[index]

        item = {
            'img_center_pth': set[0],
            'img_left_pth': set[1],
            'img_right_pth': set[2],
            'steering_angle' : float(set[3]),
            'throttle' : float(set[4]),
            'brake' : float(set[5]),
            'speed' : float(set[6])
        }

        return item

    def __len__(self):
        return len(self.data)


