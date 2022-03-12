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
            'cam' : set[0],
            'steering_angle' : float(set[1]),
            'throttle' : float(set[2]),
            'brake' : float(set[3])
        }

        return item

    def __len__(self):
        return len(self.data)


# if __name__ == '__main__':
#     #test
#     dataset = Extract()
#     ##print(dataset[0]['img'].shape)