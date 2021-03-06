###Train model 

##Libraries and Imports
import datetime
import os
import time
import itertools
import numpy as np
import torch
import torch.nn as nn
from tqdm import tqdm
tqdm.monitor_interval = 0
import torch.optim as optim
from torch.autograd import Variable
from torch.utils.data import DataLoader
from tensorboard_logger import configure, log_value

# from torch.utils.tensorboard import SummaryWriter


# current_time = datetime.datetime.now().strftime("%Y%m%d-%H%M%S")
# train_log_dir = 'logs/' + current_time + 'log'
# test_log_dir = 'logs/gradient_tape/' + current_time + '/test'
# summary_writer = SummaryWriter(train_log_dir)
# test_summary_writer = SummaryWriter(test_log_dir)

# import torchvision


from torch.utils.data.sampler import RandomSampler, SequentialSampler

import argparse
from matplotlib import pyplot as plt
import cv2

from model import CNN
from data import Extract
import utils

import pdb
import sys



#train model 
def train_model(args, model, dataset_train, dataset_val):
    model.train()
    optimizer = optim.Adam(model.parameters(), lr=1e-4)

    criterion = nn.MSELoss()    

    step = 0
    imgs_per_batch = args.batch_size
    optimizer.zero_grad()
    for epoch in range(args.nb_epoch):
        sampler = RandomSampler(dataset_train, replacement=True, num_samples=args.samples_per_epoch)
        for i, sample_id in enumerate(sampler):
            data = dataset_train[sample_id]

            label = data['steering_angle'] #, data['brake'], data['speed'], data['throttle']
            img_pth, label = utils.choose_image(label)
            img = cv2.imread(data[img_pth])
            img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
            img = utils.preprocess(img)
            img, label = utils.random_flip(img, label)
            img, label = utils.random_translate(img, label, 100, 10)
            img = utils.random_shadow(img)
            img = utils.random_brightness(img)
            img = Variable(torch.FloatTensor([img]))
            label = np.array([label]).astype(float)
            label = Variable(torch.FloatTensor(label))
            img = img.permute(0,3,1,2)


            ##write img to tensorboard
            # img_grid = torchvision.utils.make_grid(img)
            # writer.add_image('image', img_grid)
            # writer.add_graph(model, img)
            # writer.close()

            out_vec = model(img)
            loss = criterion(out_vec,label)

            loss.backward()
            if step%imgs_per_batch==0:
                optimizer.step()
                optimizer.zero_grad()


            if step%20==0:
                log_str = \
                    'Epoch: {} | Iter: {} | Step: {} | ' + \
                    'Train Loss: {:.8f} |'
                log_str = log_str.format(
                    epoch,
                    i,
                    step,
                    loss.item())
                print(log_str)

            if step%100==0:
                log_value('train_loss',loss.item(),step)
                # summary_writer.add_scalar('Loss/train', loss.item(), step)

            if step%5000==0:
                val_loss = eval_model(model,dataset_val, num_samples=3800)
                log_value('val_loss',val_loss,step)
                # summary_writer.add_scalar('Loss/test', val_loss, step)
                log_str = \
                    'Epoch: {} | Iter: {} | Step: {} | Val Loss: {:.8f}'
                log_str = log_str.format(
                    epoch,
                    i,
                    step,
                    val_loss)
                print(log_str)
                model.train()

            # if step%5000==0:
            #     if not os.path.exists(args.model_dir):
            #         os.makedirs(args.model_dir)

            #     reflex_pth = os.path.join(
            #         args.model_dir,
            #         'model_{}'.format(step))
                
            #     state = {
            #         'epoch': epoch + 1,
            #         'state_dict': model.state_dict(),
            #         'optimizer': optimizer.state_dict(),
                    
            #     }
            #     torch.save(
            #         model.state_dict(),
            #         reflex_pth)

            step += 1



    if not os.path.exists(args.model_dir):
                    os.makedirs(args.model_dir)

    reflex_pth = os.path.join(
        args.model_dir,
        'model_')
    
    state = {
        'epoch': epoch + 1,
        'state_dict': model.state_dict(),
        'optimizer': optimizer.state_dict(),
        
    }
    torch.save(
        model.state_dict(),
        reflex_pth) 

    # summary_writer.close()
    # test_summary_writer.close()  

    

        


def eval_model(model,dataset,num_samples):
    model.eval()
    criterion = nn.MSELoss()
    step = 0
    val_loss = 0
    count = 0
    sampler = RandomSampler(dataset)
    torch.manual_seed(0)
    for sample_id in tqdm(sampler):
        if step==num_samples:
            break

        data = dataset[sample_id]
        img_pth, label = utils.choose_image(data['steering_angle'])

        img = cv2.imread(data[img_pth])
        img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        img = utils.preprocess(img)
        img, label = utils.random_flip(img, label)
        img, label = utils.random_translate(img, label, 100, 10)
        img = utils.random_shadow(img)
        img = utils.random_brightness(img)
        img = Variable(torch.FloatTensor([img]))
        img = img.permute(0,3,1,2)
        label = np.array([label]).astype(float)
        label = Variable(torch.FloatTensor(label))

        out_vec = model(img)

        loss = criterion(out_vec,label)

        batch_size = 4
        val_loss += loss.data.item()
        count += batch_size
        step += 1

    val_loss = val_loss / float(count)
    return val_loss


def main(args):

    model = CNN()

    ##configure log using tensorboard_logger
    ##could also use SummaryWriter from tensor board if that is available 
    configure("log/")

    #create our dataset by extracting log file data
    dataset = Extract(args.data_dir)
    train_size = int(args.train_size * len(dataset))
    test_size = len(dataset) - train_size
    dataset_train, dataset_val = torch.utils.data.dataset.random_split(dataset,[train_size, test_size])

    # args.samples_per_epoch = len(dataset)//args.batch_size

    # print(args.samples_per_epoch)

    if(args.resume_train):
        # print("==> Loading checkpoint ...")
        # # use pre-trained model
        # checkpoint = torch.load(args.model,
        #                         map_location=lambda storage, loc: storage)

        # print("==> Loading checkpoint model successfully ...")
        # args.start_epoch = checkpoint['epoch']
        # model.load_state_dict(checkpoint['state_dict'])
        # optimizer.load_state_dict(checkpoint['optimizer'])
        # scheduler.load_state_dict(checkpoint['scheduler'])
        ###TODO
        pass
        
    else:
        train_model(args, model,dataset_train, dataset_val)

if __name__ == '__main__':

    parser = argparse.ArgumentParser()
    parser.add_argument('-d', help='data directory',        dest='data_dir',          type=str,   default='data')
    parser.add_argument('-m', help='model directory',       dest='model_dir',         type=str,   default='models')
    parser.add_argument('-t', help='train size fraction',   dest='train_size',        type=float, default=0.8)
    parser.add_argument('-k', help='drop out probability',  dest='keep_prob',         type=float, default=0.5)
    parser.add_argument('-n', help='number of epochs',      dest='nb_epoch',          type=int,   default=10)
    parser.add_argument('-s', help='samples per epoch',     dest='samples_per_epoch', type=int,   default=20000)
    parser.add_argument('-b', help='batch size',            dest='batch_size',        type=int,   default=40)
    parser.add_argument('-l', help='learning rate',         dest='learning_rate',     type=float, default=1.0e-4)
    parser.add_argument('-r', help='resume training',         dest='resume_train',     type=bool, default=False)


    args = parser.parse_args()
    main(args)