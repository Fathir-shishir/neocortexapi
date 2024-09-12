# ML22/23-13 Investigate Influence of parameter MaxNewSynapseCount - Azure Cloud Implementation
## Introduction
Temporal memory algorithms have gained popularity as a promising approach for modeling temporal sequences in machine learning. The objective of this project is to explore the effects of varying the MaxNewSynapseCount parameter within a temporal memory algorithm. This algorithm, inspired by the principles of cortical columns and the neocortex, plays a critical role in sequence learning and prediction by leveraging sparse distributed representations (SDR). Our goal is to analyze how different settings of this parameter influence the algorithm's accuracy and learning speed, particularly in complex, multi-sequence learning scenarios.

To enhance the reliability and scalability of our experiments, we are executing this project on the Azure Cloud. Running the analysis in a cloud environment offers several advantages over traditional local execution. Azure's scalable infrastructure allows us to process large datasets and execute computationally intensive tasks with greater efficiency. The cloud also facilitates seamless integration of various services, such as data storage, real-time monitoring, and automated deployments, which are crucial for handling extensive experimentation and data analysis.

Moreover, by leveraging Azure's distributed computing capabilities, we can parallelize our experiments, leading to faster iteration cycles and more comprehensive exploration of the parameter space. This approach not only improves the robustness of our findings but also ensures that our methodology can be easily scaled and adapted to future research initiatives.

In summary, implementing this project on Azure Cloud provides us with the computational power, flexibility, and scalability required to conduct a thorough investigation of the MaxNewSynapseCount parameter's impact, ultimately contributing to a deeper understanding of temporal memory algorithms in machine learning.

## Recap (Software Engineering Project)
If you need to obtain a copy of our project on your own system, use these links in order to carry out development and testing. Look at the notes on how to deploy the project and experiment with it on a live system. These are the relevant links:

- Project Documentation: [Documentation](https://github.com/Fathir-shishir/neocortexapi/blob/team_AS/source/MySEProject/EffectMaxNewSynapseCount/EffectMaxNewSynapseCount/Documentation/ML22_23-1%20Investigate%20Influence%20of%20parameter%20MaxNewSynapseCount-Team_AS.pdf) 

- Unit Test Cases: [here](https://github.com/Fathir-shishir/neocortexapi/tree/team_AS/source/MySEProject/EffectMaxNewSynapseCount/EffectMaxNewSynapseCount/EffectSynapseCountTest)

## What is this experiment about
In this experiment we have implemented our Software Engineering project in Azure cloud. Below is the total algorithm of the project:

![image](images/teamas_diagram.png)

## Information about our Azure accounts and their components

|  |  |  |
| --- | --- | --- |
| Resource Group | ```RG-teamAS``` | --- |
| Container Registry | ```teamascr``` | --- |
| Container Registry server | ```teamascr.azurecr.io``` | --- |
| Container Instance | ```teamas10th``` | --- |
| Storage account | ```teamas2storage``` | --- |
| Queue storage | ```teamasqueue``` | Queue which containes trigger message |
| Training container | ```teamastrainingcontainer``` | Container used to store training data|
| Result container | ```teamasresultcontainer``` | Container used to store result data|
| Table storage | ```teamastable``` | Table used to store all output datas and results |

The experiment Docker image can be pulled from the Azure Container Registry using the instructions below.
~~~
docker login teamascr.azurecr.io -u teamascr -p tLBIdMrZP+obaZewqvnIX3eh2SiRKtQnpr3INkSOwl+ACRBp60xL
~~~
~~~
docker pull teamascr.azurecr.io/mycloudproject:teamas
~~~

then run 
~~~
docker-compose build
~~~ 

The docker image will be build

![image](images/docker_image_build_acr.png)

## How to run the experiment
## Step1 : Message input from azure portal
at a message to queues inside Azure storage account.
p.s Uncheck "Encode the message body in Base64"

**How to add message :** 

Azure portal > Home > RG-teamAS | Queues > teamasqueue> Add message
![image](images/queue_message.png)

### Queue Message that will trigger the experiment:
~~~json
{
  "ExperimentId": "1",
  "InputFile": "run-team-as-project",
  "Description": "Team AS Cloud Computing Implementation",
  "ProjectName": "ML22/23-13 Investigate Influence of parameter MaxNewSynapseCount",
  "GroupName": "team-as",
  "Students": [ "Fathir Shishir", "Akash Saha" ],
  "file1": "file_1.json",
  "file2": "file_2.json"
}
~~~

Go to "teamas10th," "Containers," and "logs" to make sure the experiment is being run from a container instance.

when the experiment  is successful bellow message(Experiment complete successfully) will be shown. Experiment successfully

![image](images/project_run.png)

## Step2: Describe the Experiment Training Input Container

Before the experiments are starting, the input files are stored in ``` teamastrainingcontainer``` 

After the queue message received, this files are read from the container and the project is started.

![image](images/training_container.png)

## Step3: Describe the Experiment Result Output Container

after the experiments are completed, the result file is stored in Azure storage blob containers 

![image](images/ResultContainer.png)

the result data are also subsequently uploaded into a database table named "teamastable"

![image](images/result_table.png)

# Datasets Input Format:

This is how we save the dataset in a json file and upload in the blobstorage named training container and pass the file name as the value in queue message such as "file1": "file_1.json"

- **Sequences**: 
  - **S1**: [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

# Experiment Results: Cloud Environment

## Detailed Dataset Analysis

### Understanding the Chart

The chart represents the relationship between `maxNewSynapseCount` and three key metrics: **Accuracy** (blue), **Cycles to Stabilize** (green), and **Test Duration** (red).

- **Accuracy (blue)**: Indicates how well the model performed on the dataset. Higher values are better. You can observe how accuracy decreases slightly as `maxNewSynapseCount` increases.
- **Cycles to Stabilize (green)**: Reflects how quickly the model reaches a stable state. Fewer cycles mean faster stabilization. As `maxNewSynapseCount` increases, the cycles to stabilize generally decrease.
- **Test Duration (red)**: Measures the time taken for the model to run. Typically, fewer cycles result in shorter durations. Test duration corresponds closely to the number of cycles to stabilize.

### Dataset 1
- **Sequences**: 
  - **S1**: [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 8.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 180                 | 100.0        | 00:22:15           |
| 30                 | 178                 | 98.5        | 00:21:32           |
| 40                 | 163                 | 97.0        | 00:20:15           |
| 50                 | 151                 | 95.0        | 00:19:57           |

Chart for better understanding:

![image](images/dataset_chart_1.png)

### Dataset 2
- **Sequences**: 
  - **S1**: [1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 55.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 335                 | 99.0         | 00:23:47           |
| 30                 | 321                 | 98.0         | 00:22:21           |
| 40                 | 293                 | 96.0         | 00:21:56           |
| 50                 | 227                 | 93.0         | 00:20:13           |

Chart for better understanding:

![image](images/dataset_chart_2.png)

### Dataset 3
- **Sequences**: 
  - **S1**: [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 345                 | 97.0         | 00:24:25           |
| 30                 | 332                 | 96.0         | 00:23:47           |
| 40                 | 290                 | 94.0         | 00:22:34           |
| 50                 | 210                 | 90.0         | 00:21:12           |

Chart for better understanding:

![image](images/dataset_chart_3.png)

### Dataset 4
- **Sequences**: 
  - **S1**:  [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0, 16.0, 18.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 294                 | 96.3         | 00:24:12           |
| 30                 | 319                 | 95.1         | 00:25:46           |
| 40                 | 281                 | 93.8         | 00:22:58           |
| 50                 | 334                 | 91.7         | 00:27:19           |

Chart for better understanding:

![image](images/dataset_chart_4.png)

### Dataset 5
- **Sequences**: 
  - **S1**:  [2.5, 4.8, 6.1, 8.4, 10.7, 13.2, 16.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 331                 | 96.8         | 00:23:58           |
| 30                 | 315                 | 94.7         | 00:22:43           |
| 40                 | 276                 | 92.9         | 00:21:32           |
| 50                 | 251                 | 91.2         | 00:20:11           |

Chart for better understanding:

![image](images/dataset_chart_5.png)

### Dataset 6
- **Sequences**: 
  - **S1**:  [3.0, 5.5, 7.8, 9.6, 12.4, 15.1, 18.3]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 298                 | 97.5         | 00:24:35           |
| 30                 | 319                 | 95.9         | 00:25:17           |
| 40                 | 287                 | 94.2         | 00:23:47           |
| 50                 | 272                 | 92.1         | 00:22:12           |

Chart for better understanding:

![image](images/dataset_chart_6.png)

### Dataset 7
- **Sequences**: 
  - **S1**:  [2.1, 4.3, 6.5, 8.7, 10.9, 13.1, 15.3]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 312                 | 98.2         | 00:25:48           |
| 30                 | 294                 | 96.8         | 00:24:16           |
| 40                 | 279                 | 94.9         | 00:23:12           |
| 50                 | 266                 | 91.5         | 00:22:37           |

Chart for better understanding:

![image](images/dataset_chart_7.png)

### Dataset 8
- **Sequences**: 
  - **S1**:  [3.2, 5.6, 7.1, 9.8, 12.4, 14.7, 17.5]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 285                 | 97.3         | 00:26:10           |
| 30                 | 278                 | 96.1         | 00:25:34           |
| 40                 | 255                 | 94.2         | 00:24:01           |
| 50                 | 239                 | 92.0         | 00:23:29           |

Chart for better understanding:

![image](images/dataset_chart_8.png)

### Dataset 9
- **Sequences**: 
  - **S1**:  [2.0, 4.0, 8.0, 10.0, 14.0, 18.0, 22.0, 28.0, 10.0, 14.0, 18.0, 22.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 300                 | 98           | 00:27:45           |
| 30                 | 290                 | 96           | 00:26:13           |
| 40                 | 275                 | 93           | 00:25:02           |
| 50                 | 260                 | 91           | 00:23:45           |

Chart for better understanding:

![image](images/dataset_chart_9.png)

### Dataset 10
- **Sequences**: 
  - **S1**:  [7.0, 4.0, 8.0, 10.0, 14.0, 18.0, 22.0, 28.0, 10.0, 14.0, 11.0, 23.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 312                 | 95.0         | 00:24:34           |
| 30                 | 341                 | 93.0         | 00:26:12           |
| 40                 | 299                 | 92.0         | 00:23:45           |
| 50                 | 367                 | 90.0         | 00:28:10           |

Chart for better understanding:

![image](images/dataset_chart_10.png)

### Dataset 11
- **Sequences**: 
  - **S1**:  [3.0, 6.0, 12.0, 16.0, 20.0, 25.0, 30.0, 35.0, 40.0, 20.0, 15.0, 10.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 350                 | 94.0         | 00:26:45           |
| 30                 | 330                 | 92.0         | 00:25:22           |
| 40                 | 310                 | 91.0         | 00:24:10           |
| 50                 | 375                 | 89.0         | 00:28:55           |

Chart for better understanding:

![image](images/dataset_chart_11.png)

### Dataset 12
- **Sequences**: 
  - **S1**:  [4.0, 8.0, 12.0, 16.0, 24.0, 32.0, 20.0, 10.0, 28.0, 30.0, 36.0, 18.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 340                 | 95.0         | 00:25:38           |
| 30                 | 325                 | 94.0         | 00:24:55           |
| 40                 | 310                 | 92.0         | 00:23:22           |
| 50                 | 355                 | 90.0         | 00:27:14           |

Chart for better understanding:

![image](images/dataset_chart_12.png)

### Dataset 13
- **Sequences**: 
  - **S1**: [3.0, 7.0, 11.0, 15.0, 21.0, 25.0, 19.0, 13.0, 27.0, 31.0, 37.0, 23.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 315                 | 96.0         | 00:23:45           |
| 30                 | 290                 | 94.0         | 00:22:10           |
| 40                 | 275                 | 93.0         | 00:21:05           |
| 50                 | 246                 | 91.0         | 00:20:32           |

Chart for better understanding:

![image](images/dataset_chart_13.png)

### Dataset 14
- **Sequences**: 
  - **S1**: [5.0, 9.0, 13.0, 17.0, 21.0, 29.0, 35.0, 41.0, 49.0, 33.0, 23.0, 19.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 325                 | 97.0         | 00:24:10           |
| 30                 | 310                 | 95.5         | 00:23:00           |
| 40                 | 295                 | 94.0         | 00:21:50           |
| 50                 | 370                 | 100.0         | 00:27:20           |

Chart for better understanding:

![image](images/dataset_chart_14.png)

### Dataset 15
- **Sequences**: 
  - **S1**: [7.0, 12.0, 16.0, 20.0, 25.0, 30.0, 35.0, 40.0, 50.0, 60.0, 70.0, 80.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 340                 | 98.0         | 00:25:32           |
| 30                 | 320                 | 96.0         | 00:24:10           |
| 40                 | 295                 | 94.5         | 00:23:05           |
| 50                 | 380                 | 91.0         | 00:28:30           |

Chart for better understanding:

![image](images/dataset_chart_15.png)

### Dataset 16
- **Sequences**: 
  - **S1**: [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0, 24.0, 27.0, 30.0, 33.0, 36.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 360                 | 97.5         | 00:26:15           |
| 30                 | 345                 | 96.0         | 00:24:59           |
| 40                 | 310                 | 94.0         | 00:23:50           |
| 50                 | 380                 | 92.0         | 00:29:35           |


Chart for better understanding:

![image](images/dataset_chart_16.png)

### Dataset 17
- **Sequences**: 
  - **S1**: [5.0, 7.0, 10.0, 12.0, 17.0, 20.0, 25.0, 30.0, 35.0, 40.0, 50.0, 60.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 375                 | 100         | 00:27:10           |
| 30                 | 342                 | 95.3         | 00:25:48           |
| 40                 | 305                 | 93.5         | 00:23:33           |
| 50                 | 385                 | 100         | 00:21:55           |


Chart for better understanding:

![image](images/dataset_chart_17.png)

### Dataset 18
- **Sequences**: 
  - **S1**: [6.0, 9.0, 15.0, 18.0, 22.0, 27.0, 33.0, 39.0, 44.0, 51.0, 60.0, 72.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 390                 | 97.1         | 00:28:10           |
| 30                 | 355                 | 95.5         | 00:26:48           |
| 40                 | 322                 | 94.0         | 00:24:15           |
| 50                 | 300                 | 92.0         | 00:20:20           |


Chart for better understanding:

![image](images/dataset_chart_18.png)

### Dataset 19
- **Sequences**: 
  - **S1**: [5.0, 12.0, 16.0, 21.0, 28.0, 34.0, 40.0, 48.0, 55.0, 63.0, 70.0, 80.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 370                 | 100.0         | 00:27:45           |
| 30                 | 355                 | 94.5         | 00:25:59           |
| 40                 | 330                 | 100.0         | 00:24:50           |
| 50                 | 410                 | 100.0         | 00:22:17           |


Chart for better understanding:

![image](images/dataset_chart_19.png)

### Dataset 20
- **Sequences**: 
  - **S1**: [6.0, 14.0, 20.0, 26.0, 34.0, 42.0, 50.0, 58.0, 65.0, 73.0, 82.0, 90.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 365                 | 95.4         | 00:26:22           |
| 30                 | 341                 | 94.1         | 00:24:58           |
| 40                 | 319                 | 92.8         | 00:23:43           |
| 50                 | 293                 | 93.0         | 00:20:18           |


Chart for better understanding:

![image](images/dataset_chart_20.png)

### Dataset 21
- **Sequences**: 
  - **S1**: [7.0, 14.0, 20.0, 29.0, 36.0, 45.0, 53.0, 62.0, 72.0, 85.0, 95.0, 100.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 390                 | 97.1         | 00:28:50           |
| 30                 | 372                 | 95.9         | 00:26:47           |
| 40                 | 340                 | 92.3         | 00:25:30           |
| 50                 | 415                 | 91.0         | 00:34:05           |


Chart for better understanding:

![image](images/dataset_chart_21.png)

### Dataset 22
- **Sequences**: 
  - **S1**: [7.0, 14.0, 20.0, 29.0, 36.0, 45.0, 53.0, 62.0, 72.0, 85.0, 95.0, 100.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 420                 | 100.0         | 00:30:15           |
| 30                 | 375                 | 100.0         | 00:27:45           |
| 40                 | 360                 | 84.7         | 00:26:50           |
| 50                 | 355                 | 100.0         | 00:16:10           |


Chart for better understanding:

![image](images/dataset_chart_22.png)

### Dataset 23
- **Sequences**: 
  - **S1**: [3.0, 6.0, 12.0, 18.0, 24.0, 31.0, 39.0, 49.0, 61.0, 74.0, 89.0, 105.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 320                 | 100.0        | 00:28:10           |
| 30                 | 315                 | 100.0        | 00:26:50           |
| 40                 | 345                 | 97.5         | 00:30:35           |
| 50                 | 380                 | 93.8         | 00:33:45           |


Chart for better understanding:

![image](images/dataset_chart_23.png)

### Dataset 24
- **Sequences**: 
  - **S1**: [5.0, 10.0, 15.0, 25.0, 35.0, 45.0, 55.0, 70.0, 85.0, 100.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 303                 | 100.0        | 00:27:45           |
| 30                 | 290                 | 100.0        | 00:26:10           |
| 40                 | 267                 | 96.5         | 00:29:20           |
| 50                 | 261                 | 94.2         | 00:22:30           |


Chart for better understanding:

![image](images/dataset_chart_24.png)

### Dataset 25
- **Sequences**: 
  - **S1**: [4.0, 8.0, 12.0, 16.0, 20.0, 24.0, 28.0, 32.0, 36.0, 40.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 280                 | 100.0        | 00:26:12           |
| 30                 | 275                 | 100.0        | 00:25:05           |
| 40                 | 300                 | 95.8         | 00:27:50           |
| 50                 | 290                 | 94.5         | 00:24:30           |


Chart for better understanding:

![image](images/dataset_chart_25.png)

### Dataset 26
- **Sequences**: 
  - **S1**: [4.0, 8.0, 12.0, 16.0, 20.0, 24.0, 28.0, 32.0, 36.0, 40.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 265                 | 100.0        | 00:25:20           |
| 30                 | 270                 | 100.0        | 00:24:35           |
| 40                 | 295                 | 96.7         | 00:26:45           |
| 50                 | 285                 | 94.8         | 00:24:10           |


Chart for better understanding:

![image](images/dataset_chart_26.png)

### Dataset 27
- **Sequences**: 
  - **S1**: [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0, 45.0, 50.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 280                 | 100.0        | 00:25:45           |
| 30                 | 290                 | 99.8         | 00:26:10           |
| 40                 | 320                 | 96.4         | 00:28:15           |
| 50                 | 300                 | 94.9         | 00:27:05           |


Chart for better understanding:

![image](images/dataset_chart_27.png)

### Dataset 28
- **Sequences**: 
  - **S1**: [6.0, 12.0, 18.0, 24.0, 30.0, 36.0, 42.0, 48.0, 54.0, 60.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 270                 | 100.0        | 00:24:11           |
| 30                 | 290                 | 99.7         | 00:25:33           |
| 40                 | 310                 | 96.9         | 00:26:47           |
| 50                 | 280                 | 95.5         | 00:24:58           |


Chart for better understanding:

![image](images/dataset_chart_28.png)

### Dataset 29
- **Sequences**: 
  - **S1**: [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0, 45.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 261                 | 100.0        | 00:23:07           |
| 30                 | 287                 | 99.8         | 00:24:43           |
| 40                 | 293                 | 97.5         | 00:26:17           |
| 50                 | 277                 | 95.0         | 00:25:11           |


Chart for better understanding:

![image](images/dataset_chart_29.png)

### Dataset 30
- **Sequences**: 
  - **S1**: [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0]
- **MaxNewSynapseCount1**: 20
- **MaxNewSynapseCount2**: 30
- **MaxNewSynapseCount1**: 40
- **MaxNewSynapseCount2**: 50

| MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|--------------------|---------------------|--------------|--------------------|
| 20                 | 245                 | 100.0        | 00:22:15           |
| 30                 | 265                 | 99.8         | 00:24:35           |
| 40                 | 289                 | 96.7         | 00:26:10           |
| 50                 | 255                 | 95.2         | 00:23:50           |


Chart for better understanding:

![image](images/dataset_chart_30.png)



## Cummulative Result Table

Here we provide 120 variants of sequences with 4 different maxNewSynapseCount values to evaluate the influence of maxNewSynapseCount

| Sequence                                    | MaxNewSynapseCount | Cycles to Stabilize | Accuracy (%) | Test Duration      |
|---------------------------------------------|--------------------|---------------------|--------------|--------------------|
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 8.0] | 20                 | 180                 | 100.0        | 00:22:15           |
|                                             | 30                 | 178                 | 98.5         | 00:21:32           |
|                                             | 40                 | 163                 | 97.0         | 00:20:15           |
|                                             | 50                 | 151                 | 95.0         | 00:19:57           |
| [1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 55.0] | 20                 | 335                 | 99.0         | 00:23:47           |
|                                             | 30                 | 321                 | 98.0         | 00:22:21           |
|                                             | 40                 | 293                 | 96.0         | 00:21:56           |
|                                             | 50                 | 227                 | 93.0         | 00:20:13           |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0] | 20                 | 345                 | 97.0         | 00:24:25           |
|                                             | 30                 | 332                 | 96.0         | 00:23:47           |
|                                             | 40                 | 290                 | 94.0         | 00:22:34           |
|                                             | 50                 | 210                 | 90.0         | 00:21:12           |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0, 16.0, 18.0] | 20                 | 294                 | 96.3         | 00:24:12           |
|                                             | 30                 | 319                 | 95.1         | 00:25:46           |
|                                             | 40                 | 281                 | 93.8         | 00:22:58           |
|                                             | 50                 | 334                 | 91.7         | 00:27:19           |
| [2.5, 4.8, 6.1, 8.4, 10.7, 13.2, 16.0]      | 20                 | 331                 | 96.8         | 00:23:58           |
|                                             | 30                 | 315                 | 94.7         | 00:22:43           |
|                                             | 40                 | 276                 | 92.9         | 00:21:32           |
|                                             | 50                 | 251                 | 91.2         | 00:20:11           |
| [3.0, 5.5, 7.8, 9.6, 12.4, 15.1, 18.3]      | 20                 | 298                 | 97.5         | 00:24:35           |
|                                             | 30                 | 319                 | 95.9         | 00:25:17           |
|                                             | 40                 | 287                 | 94.2         | 00:23:47           |
|                                             | 50                 | 272                 | 92.1         | 00:22:12           |
| [2.1, 4.3, 6.5, 8.7, 10.9, 13.1, 15.3]      | 20                 | 312                 | 98.2         | 00:25:48           |
|                                             | 30                 | 294                 | 96.8         | 00:24:16           |
|                                             | 40                 | 279                 | 94.9         | 00:23:12           |
|                                             | 50                 | 266                 | 91.5         | 00:22:37           |
| [3.2, 5.6, 7.1, 9.8, 12.4, 14.7, 17.5]      | 20                 | 285                 | 97.3         | 00:26:10           |
|                                             | 30                 | 278                 | 96.1         | 00:25:34           |
|                                             | 40                 | 255                 | 94.2         | 00:24:01           |
|                                             | 50                 | 239                 | 92.0         | 00:23:29           |
| [2.0, 4.0, 8.0, 10.0, 14.0, 18.0, 22.0, 28.0, 10.0, 14.0, 18.0, 22.0] | 20                 | 300                 | 98            | 00:27:45           |
|                                             | 30                 | 290                 | 96            | 00:26:13           |
|                                             | 40                 | 275                 | 93            | 00:25:02           |
|                                             | 50                 | 260                 | 91            | 00:23:45           |
| [7.0, 4.0, 8.0, 10.0, 14.0, 18.0, 22.0, 28.0, 10.0, 14.0, 11.0, 23.0] | 20                 | 312                 | 95.0          | 00:24:34           |
|                                             | 30                 | 341                 | 93.0          | 00:26:12           |
|                                             | 40                 | 299                 | 92.0          | 00:23:45           |
|                                             | 50                 | 367                 | 90.0          | 00:28:10           |
| [3.0, 6.0, 12.0, 16.0, 20.0, 25.0, 30.0, 35.0, 40.0, 20.0, 15.0, 10.0] | 20                 | 350                 | 94.0          | 00:26:45           |
|                                             | 30                 | 330                 | 92.0          | 00:25:22           |
|                                             | 40                 | 310                 | 91.0          | 00:24:10           |
|                                             | 50                 | 375                 | 89.0          | 00:28:55           |
| [4.0, 8.0, 12.0, 16.0, 24.0, 32.0, 20.0, 10.0, 28.0, 30.0, 36.0, 18.0] | 20                 | 340                 | 95.0          | 00:25:38           |
|                                             | 30                 | 325                 | 94.0          | 00:24:55           |
|                                             | 40                 | 310                 | 92.0          | 00:23:22           |
|                                             | 50                 | 355                 | 90.0          | 00:27:14           |
| [3.0, 7.0, 11.0, 15.0, 21.0, 25.0, 19.0, 13.0, 27.0, 31.0, 37.0, 23.0] | 20                 | 315                 | 96.0          | 00:23:45           |
|                                             | 30                 | 290                 | 94.0          | 00:22:10           |
|                                             | 40                 | 275                 | 93.0          | 00:21:05           |
|                                             | 50                 | 246                 | 91.0          | 00:20:32           |
| [5.0, 9.0, 13.0, 17.0, 21.0, 29.0, 35.0, 41.0, 49.0, 33.0, 23.0, 19.0] | 20                 | 325                 | 97.0          | 00:24:10           |
|                                             | 30                 | 310                 | 95.5          | 00:23:00           |
|                                             | 40                 | 295                 | 94.0          | 00:21:50           |
|                                             | 50                 | 370                 | 100.0         | 00:27:20           |
| [7.0, 12.0, 16.0, 20.0, 25.0, 30.0, 35.0, 40.0, 50.0, 60.0, 70.0, 80.0] | 20                 | 340                 | 98.0          | 00:25:32           |
|                                             | 30                 | 320                 | 96.0          | 00:24:10           |
|                                             | 40                 | 295                 | 94.5          | 00:23:05           |
|                                             | 50                 | 380                 | 91.0          | 00:28:30           |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 8.0] | 20                 | 180                 | 100.0        | 00:22:15           |
|                                              | 30                 | 178                 | 98.5         | 00:21:32           |
|                                              | 40                 | 163                 | 97.0         | 00:20:15           |
|                                              | 50                 | 151                 | 95.0         | 00:19:57           |
| [1.0, 2.0, 3.0, 5.0, 8.0, 13.0, 21.0, 34.0, 55.0] | 20                 | 335                 | 99.0         | 00:23:47           |
|                                              | 30                 | 321                 | 98.0         | 00:22:21           |
|                                              | 40                 | 293                 | 96.0         | 00:21:56           |
|                                              | 50                 | 227                 | 93.0         | 00:20:13           |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0, 24.0, 27.0, 30.0, 33.0, 36.0] | 20                 | 360                 | 97.5         | 00:26:15           |
|                                              | 30                 | 345                 | 96.0         | 00:24:59           |
|                                              | 40                 | 310                 | 94.0         | 00:23:50           |
|                                              | 50                 | 380                 | 92.0         | 00:29:35           |
| [5.0, 7.0, 10.0, 12.0, 17.0, 20.0, 25.0, 30.0, 35.0, 40.0, 50.0, 60.0] | 20                 | 375                 | 100          | 00:27:10           |
|                                              | 30                 | 342                 | 95.3         | 00:25:48           |
|                                              | 40                 | 305                 | 93.5         | 00:23:33           |
|                                              | 50                 | 385                 | 100          | 00:21:55           |
| [6.0, 9.0, 15.0, 18.0, 22.0, 27.0, 33.0, 39.0, 44.0, 51.0, 60.0, 72.0] | 20                 | 390                 | 97.1         | 00:28:10           |
|                                              | 30                 | 355                 | 95.5         | 00:26:48           |
|                                              | 40                 | 322                 | 94.0         | 00:24:15           |
|                                              | 50                 | 300                 | 92.0         | 00:20:20           |
| [5.0, 12.0, 16.0, 21.0, 28.0, 34.0, 40.0, 48.0, 55.0, 63.0, 70.0, 80.0] | 20                 | 370                 | 100.0        | 00:27:45           |
|                                              | 30                 | 355                 | 94.5         | 00:25:59           |
|                                              | 40                 | 330                 | 100.0        | 00:24:50           |
|                                              | 50                 | 410                 | 100.0        | 00:22:17           |
| [6.0, 14.0, 20.0, 26.0, 34.0, 42.0, 50.0, 58.0, 65.0, 73.0, 82.0, 90.0] | 20                 | 365                 | 95.4         | 00:26:22           |
|                                              | 30                 | 341                 | 94.1         | 00:24:58           |
|                                              | 40                 | 319                 | 92.8         | 00:23:43           |
|                                              | 50                 | 293                 | 93.0         | 00:20:18           |
| [7.0, 14.0, 20.0, 29.0, 36.0, 45.0, 53.0, 62.0, 72.0, 85.0, 95.0, 100.0] | 20                 | 390                 | 97.1         | 00:28:50           |
|                                              | 30                 | 372                 | 95.9         | 00:26:47           |
|                                              | 40                 | 340                 | 92.3         | 00:25:30           |
|                                              | 50                 | 415                 | 91.0         | 00:34:05           |
| [7.0, 14.0, 20.0, 29.0, 36.0, 45.0, 53.0, 62.0, 72.0, 85.0, 95.0, 100.0] | 20                 | 420                 | 100.0        | 00:30:15           |
|                                              | 30                 | 375                 | 100.0        | 00:27:45           |
|                                              | 40                 | 360                 | 84.7         | 00:26:50           |
|                                              | 50                 | 355                 | 100.0        | 00:16:10           |
| [3.0, 6.0, 12.0, 18.0, 24.0, 31.0, 39.0, 49.0, 61.0, 74.0, 89.0, 105.0] | 20                 | 320                 | 100.0        | 00:28:10           |
|                                              | 30                 | 315                 | 100.0        | 00:26:50           |
|                                              | 40                 | 345                 | 97.5         | 00:30:35           |
|                                              | 50                 | 380                 | 93.8         | 00:33:45           |
| [5.0, 10.0, 15.0, 25.0, 35.0, 45.0, 55.0, 70.0, 85.0, 100.0] | 20                 | 303                 | 100.0        | 00:27:45           |
|                                              | 30                 | 290                 | 100.0        | 00:26:10           |
|                                              | 40                 | 267                 | 96.5         | 00:29:20           |
|                                              | 50                 | 261                 | 94.2         | 00:22:30           |
| [4.0, 8.0, 12.0, 16.0, 20.0, 24.0, 28.0, 32.0, 36.0, 40.0] | 20                 | 280                 | 100.0        | 00:26:12           |
|                                              | 30                 | 275                 | 100.0        | 00:25:05           |
|                                              | 40                 | 300                 | 95.8         | 00:27:50           |
|                                              | 50                 | 290                 | 94.5         | 00:24:30           |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0] | 20                 | 245                 | 100.0        | 00:22:15           |
|                                              | 30                 | 265                 | 99.8         | 00:24:35           |
|                                              | 40                 | 289                 | 96.7         | 00:26:10           |
|                                              | 50                 | 255                 | 95.2         | 00:23:50           |
| [6.0, 12.0, 18.0, 24.0, 30.0, 36.0, 42.0, 48.0, 54.0, 60.0] | 20                 | 270                 | 100.0        | 00:24:11           |
|                                              | 30                 | 290                 | 99.7         | 00:25:33           |
|                                              | 40                 | 310                 | 96.9         | 00:26:47           |
|                                              | 50                 | 280                 | 95.5         | 00:24:58           |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0] | 20                 | 245                 | 100.0        | 00:22:15           |
|                                              | 30                 | 265                 | 99.8         | 00:24:35           |
|                                              | 40                 | 289                 | 96.7         | 00:26:10           |
|                                              | 50                 | 255                 | 95.2         | 00:23:50           |

### By Reduced max cycles count and reduced matching count

We also ran 200 sequence with different maxNewSynapseCount with decreased cycles count and reduced matching count for faster stable state, it did make our work and results getting much more faster but we did loose some accuracy whereas when we had more it was getting much longer time. So it was a tradeoff between test duration and accuracy.  

| Sequence                                                                                                                                                                    | MaxNewSynapseCount | Accuracy (%) | Test Duration (minutes) |
|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------|--------------|-------------------------|
| [0.0, 1.0, 0.0, 2.0, 3.0, 4.0, 5.0, 6.0, 5.0, 4.0, 3.0, 7.0, 1.0, 9.0, 12.0, 11.0, 12.0, 13.0, 14.0, 11.0, 12.0, 14.0, 5.0, 7.0, 6.0, 9.0, 3.0, 4.0, 3.0, 4.0, 3.0, 4.0] | 20                 | 89.75        | 07:12                   |
| [0.8, 2.0, 0.0, 3.0, 3.0, 4.0, 5.0, 6.0, 5.0, 7.0, 2.0, 7.0, 1.0, 9.0, 11.0, 11.0, 10.0, 13.0, 14.0, 11.0, 7.0, 6.0, 5.0, 7.0, 6.0, 5.0, 3.0, 2.0, 3.0, 4.0, 3.0, 4.0] | 30                 | 91.25        | 08:05                   |
| [2.1, 3.2, 5.0, 6.0, 8.0, 7.0, 5.0, 6.0, 9.0, 12.0, 14.0, 15.0, 16.0, 18.0, 19.0, 21.0, 22.0, 24.0, 23.0, 25.0]                                                             | 40                 | 81.45        | 07:05                   |
| [0.0, 1.0, 2.0, 4.0, 5.0, 6.0, 5.0, 4.0, 7.0, 8.0, 9.0, 12.0, 10.0, 14.0, 12.0, 16.0, 15.0, 17.0, 19.0, 18.0]                                                              | 50                 | 76.92        | 06:50                   |
| [1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5, 10.0]                                                                         | 20                 | 91.00        | 06:50                   |
| [2.0, 3.0, 5.0, 6.0, 7.0, 9.0, 12.0, 11.0, 14.0, 13.0, 16.0, 17.0, 19.0, 20.0, 22.0]                                                                                        | 30                 | 83.75        | 06:25                   |
| [0.0, 1.0, 0.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0]                                                                                                     | 40                 | 79.50        | 06:40                   |
| [4.0, 5.0, 7.0, 8.0, 9.0, 11.0, 12.0, 14.0, 13.0, 16.0, 15.0, 18.0, 17.0, 19.0, 22.0, 21.0, 23.0, 24.0]                                                                     | 50                 | 83        | 07:10                   |
| [1.0, 2.0, 3.0, 3.5, 4.0, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5, 10.0]                                                                                          | 20                 | 84.12        | 06:20                   |
| [2.5, 3.5, 4.0, 5.0, 6.5, 7.0, 8.5, 9.0, 10.0, 12.0, 11.0, 13.0, 15.0, 14.0, 16.0]                                                                                          | 30                 | 87.33        | 07:00                   |
| [1.0, 3.0, 2.0, 4.0, 6.0, 5.0, 7.0, 8.0, 10.0, 11.0, 13.0, 12.0, 15.0, 14.0]                                                                                                | 40                 | 83.21        | 07:25                   |
| [2.0, 1.0, 3.0, 5.0, 4.0, 6.0, 7.0, 9.0, 8.0, 10.0, 11.0, 13.0, 12.0, 14.0]                                                                                                | 50                 | 78.12        | 07:45                   |
| [0.0, 1.0, 3.0, 4.0, 2.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 12.0, 11.0, 13.0, 15.0]                                                                                           | 20                 | 92.45        | 06:40                   |
| [1.0, 2.0, 3.0, 4.0, 6.0, 5.0, 7.0, 9.0, 8.0, 11.0, 10.0, 13.0, 12.0]                                                                                                       | 30                 | 85.50        | 06:25                   |
| [3.0, 4.0, 5.0, 6.0, 8.0, 7.0, 10.0, 9.0, 12.0, 11.0, 14.0, 13.0]                                                                                                           | 40                 | 80.75        | 07:10                   |
| [2.0, 3.0, 4.0, 5.0, 7.0, 6.0, 8.0, 9.0, 11.0, 10.0, 12.0, 13.0]                                                                                                           | 50                 | 82.50        | 07:35                   |
| [0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 12.0]                                                                                                             | 20                 | 90.35        | 06:20                   |
| [1.0, 2.0, 4.0, 5.0, 7.0, 6.0, 8.0, 10.0, 9.0, 12.0, 11.0, 14.0, 13.0]                                                                                                     | 30                 | 83.33        | 06:50                   |
| [0.8, 1.0, 3.0, 2.0, 5.0, 4.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0]                                                                                                             | 40                 | 79.12        | 06:55                   |
| [2.0, 4.0, 3.0, 5.0, 6.0, 8.0, 7.0, 10.0, 9.0, 12.0, 11.0, 13.0]                                                                                                           | 50                 | 78.12        | 07:50                   |
| [0.0, 1.0, 3.0, 4.0, 2.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                                                                                   | 20                 | 84.12        | 06:50                   |
| [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                         | 30                 | 83.33        | 06:45                   |
| [2.0, 4.0, 3.0, 5.0, 7.0, 6.0, 9.0, 8.0, 11.0, 10.0]                                                                                                                       | 40                 | 81.12        | 07:10                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                                                                                       | 50                 | 78.12        | 07:45                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                             | 20                 | 84.12        | 06:55                   |
| [2.0, 4.0, 3.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                                   | 30                 | 83.33        | 06:35                   |
| [0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0]                                                                                                                                   | 40                 | 81.12        | 07:20                   |
| [3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                                                                                                  | 50                 | 78.12        | 07:55                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0]                                                                                                                                       | 20                 | 84.12        | 06:10                   |
| [2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0]                                                                                                                                       | 30                 | 83.33        | 06:40                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                                                                                     | 20                 | 95.50        | 08:10                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0]                                                                                                                   | 30                 | 91.75        | 07:35                   |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0]                                                                                                                             | 40                 | 84.80        | 06:50                   |
| [7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0, 23.0, 25.0, 27.0]                                                                                                            | 50                 | 78.25        | 07:20                   |
| [1.0, 2.0, 4.0, 5.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                     | 20                 | 97.25        | 08:00                   |
| [0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5]                                                                                                                   | 30                 | 89.60        | 07:15                   |
| [2.0, 4.0, 3.0, 5.0, 6.0, 7.0, 9.0, 8.0, 10.0]                                                                                                                             | 40                 | 82.35        | 07:00                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0, 24.0]                                                                                                                              | 50                 | 80.12        | 07:40                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5, 17.5, 19.5]                                                                                                                    | 20                 | 96.53        | 07:51                   |
| [4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0]                                                                                                                           | 30                 | 88.51        | 07:05                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0, 20.0, 22.0]                                                                                                             | 40                 | 81.50        | 06:49                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 12.0, 11.0]                                                                                                            | 50                 | 79.72        | 07:37                   |
| [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0]                                                                                                                              | 20                 | 98.08        | 08:04                   |
| [1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0]                                                                                                                              | 30                 | 89.26        | 07:23                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0, 24.0, 27.0]                                                                                                                        | 40                 | 82.03        | 07:11                   |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0, 40.0, 45.0]                                                                                                                      | 50                 | 78.84        | 07:41                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0]                                                                                                              | 20                 | 95.22        | 08:13                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0]                                                                                                                         | 30                 | 90.34        | 07:34                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0]                                                                                                                              | 40                 | 85.27        | 07:17                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0, 23.0, 25.0]                                                                                                            | 50                 | 81.82        | 07:58                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5]                                                                                                                              | 20                 | 96.11        | 07:52                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0, 24.0, 27.0]                                                                                                                        | 30                 | 89.60        | 07:31                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                                                                                       | 40                 | 80        | 07:17                   |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0, 35.0]                                                                                                                                  | 50                 | 79        | 07:44                   |
| [1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5, 9.5]                                                                                                                              | 20                 | 97        | 08:00                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                             | 30                 | 90.13        | 07:23                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                                    | 40                 | 84.27        | 07:03                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0, 23.0, 25.0, 27.0]                                                                                                      | 50                 | 80.90        | 07:32                   |
| [0.0, 2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0]                                                                                                                   | 20                 | 94.23        | 07:51                   |
| [2.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                                                                                    | 30                 | 90.71        | 07:33                   |
| [4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                                                                                                      | 40                 | 83.71        | 07:21                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                                                                                                    | 50                 | 78.86        | 07:43                   |
| [2.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0]                                                                                                                                      | 20                 | 96.51        | 08:13                   |
| [1.0, 2.0, 4.0, 5.0, 7.0, 9.0, 11.0]                                                                                                                                      | 30                 | 88.72        | 07:11                   |
| [5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0]                                                                                                                   | 40                 | 82.73        | 07:02                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5]                                                                                                                                       | 50                 | 79.55        | 07:43                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0]                                                                                                                                  | 20                 | 98.00        | 07:51                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                                    | 30                 | 90.60        | 07:33                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                                                                                                    | 40                 | 85.13        | 07:24                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                                                                                         | 50                 | 78.13        | 07:56                   |
| [2.0, 5.0, 7.0, 9.0, 12.0, 15.0, 18.0, 20.0]                                                                                                                               | 20                 | 94        | 07:56                   |
| [1.0, 3.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                          | 30                 | 91.62        | 07:35                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0]                                                                                                                                     | 40                 | 84.59        | 07:12                   |
| [1.0, 5.0, 7.0, 11.0, 13.0, 17.0, 20.0, 25.0, 28.0]                                                                                                                        | 50                 | 79.81        | 07:45                   |
| [0.0, 2.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0]                                                                                                                          | 20                 | 95.23        | 08:00                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                                    | 30                 | 90.77        | 07:20                   |
| [1.0, 3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                                                                                               | 40                 | 82.97        | 06:50                   |
| [3.0, 5.0, 7.0, 10.0, 13.0, 17.0, 20.0]                                                                                                                                    | 50                 | 78.61        | 07:21                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5, 17.5, 19.5]                                                                                                                    | 20                 | 94.53        | 07:52                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                               | 30                 | 89.81        | 07:10                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                                                                                                    | 40                 | 85.13        | 06:55                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                                                                                                    | 50                 | 79.90        | 07:35                   |
| [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0]                                                                                                                   | 20                 | 96.00        | 08:05                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                                    | 30                 | 89.25        | 07:23                   |
| [1.0, 3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                                                                                     | 40                 | 84.13        | 07:01                   |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0]                                                                                                                                        | 50                 | 78.12        | 07:43                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0]                                                                                                                                | 20                 | 95.17        | 07:50                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                                                                                           | 30                 | 91.10        | 07:30                   |
| [3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                                                                                         | 40                 | 83.77        | 06:43                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5]                                                                                                                                | 50                 | 79.77        | 07:51                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5]                                                                                                                                   | 20                 | 94.60        | 07:55                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                             | 30                 | 94.60        | 07:14                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0]                                                                                                                              | 40                 | 85.13        | 06:54                   |
| [5.0, 10.0, 15.0, 20.0, 25.0]                                                                                                                                              | 50                 | 78.27        | 07:35                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                                                                                          | 20                 | 95.50        | 08:01                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                                                                               | 30                 | 95.50        | 07:34                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                                                                                         | 40                 | 83.51        | 07:07                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                                                                                              | 50                 | 83.51        | 07:43                   |
| [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                                                                        | 20                 | 95.75        | 08:15                   |
| [1.0, 2.0, 4.0, 5.0, 7.0, 9.0, 11.0, 13.0]                                                                                                                                 | 30                 | 95.75        | 07:20                   |
| [2.0, 3.0, 4.0, 6.0, 7.0, 9.0, 10.0, 12.0]                                                                                                                                 | 40                 | 84.17        | 07:10                   |
| [5.0, 7.0, 9.0, 12.0, 15.0, 18.0]                                                                                                                                         | 50                 | 79.21        | 07:55                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5, 17.5]                                                                                                                          | 20                 | 94.25        | 07:51                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5]                                                                                                                              | 30                 | 94.25        | 07:33                   |
| [3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0]                                                                                                                   | 40                 | 84.90        | 07:07                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                                                                                           | 50                 | 78.45        | 07:31                   |
| [2.0, 5.0, 7.0, 9.0, 12.0, 15.0, 18.0, 20.0]                                                              | 20                 | 94.30        | 07:53                   |
| [1.0, 3.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                         | 30                 | 94.30        | 07:38                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0]                                                                    | 40                 | 94.30        | 07:17                   |
| [1.0, 5.0, 7.0, 11.0, 13.0, 17.0, 20.0, 25.0, 28.0]                                                       | 50                 | 78.80        | 07:42                   |
| [0.0, 2.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0]                                                         | 20                 | 96.25        | 08:06                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                   | 30                 | 96.25        | 07:23                   |
| [1.0, 3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                              | 40                 | 78.62        | 06:57                   |
| [3.0, 5.0, 7.0, 10.0, 13.0, 17.0, 20.0]                                                                   | 50                 | 78.62        | 07:29                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5, 17.5, 19.5]                                                   | 20                 | 94.53        | 07:59                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                              | 30                 | 94.53        | 07:16                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                                   | 40                 | 85.11        | 06:52                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                                   | 50                 | 85.11        | 07:31                   |
| [0.0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0]                                                  | 20                 | 96.06        | 08:08                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                                   | 30                 | 96.06        | 07:29                   |
| [1.0, 3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                    | 40                 | 89.21        | 07:03                   |
| [5.0, 10.0, 15.0, 20.0, 25.0, 30.0]                                                                       | 50                 | 89.21        | 07:49                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0]                                                               | 20                 | 95.02        | 07:55                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                          | 30                 | 95.02        | 07:28                   |
| [3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                        | 40                 | 83.77        | 06:46                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5]                                                               | 50                 | 79.72        | 07:54                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5]                                                                  | 20                 | 94.63        | 07:58                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                             | 30                 | 94.63        | 07:19                   |
| [4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0, 18.0]                                                             | 40                 | 85.07        | 06:59                   |
| [5.0, 10.0, 15.0, 20.0, 25.0]                                                                             | 50                 | 78.52        | 07:38                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                         | 20                 | 95.55        | 08:02                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0]                                                              | 30                 | 94.63        | 07:33                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                         | 40                 | 94.63        | 07:08                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0]                                                             | 50                 | 79.60        | 07:48                   |
| [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                       | 20                 | 95.75        | 08:14                   |
| [1.0, 2.0, 4.0, 5.0, 7.0, 9.0, 11.0, 13.0]                                                                | 30                 | 89.63        | 07:26                   |
| [2.0, 3.0, 4.0, 6.0, 7.0, 9.0, 10.0, 12.0]                                                                | 40                 | 84.10        | 07:12                   |
| [5.0, 7.0, 9.0, 12.0, 15.0, 18.0]                                                                         | 50                 | 79.23        | 07:52                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5, 17.5]                                                         | 20                 | 94.28        | 07:57                   |
| [0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5, 7.5, 8.5]                                                             | 30                 | 91.25        | 07:28                   |
| [3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0]                                                  | 40                 | 84.90        | 07:11                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                          | 50                 | 84.90        | 07:32                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0]                                                        | 20                 | 95.40        | 08:12                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0]                                                                     | 30                 | 84.90        | 07:28                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                         | 40                 | 84.65        | 07:01                   |
| [5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0]                                                                    | 50                 | 79.85        | 07:38                   |
| [2.0, 3.5, 5.0, 7.0, 9.0, 11.5, 13.0, 15.0]                                                                | 20                 | 94.75        | 07:59                   |
| [1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0]                                                                      | 30                 | 89.40        | 07:23                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5]                                                                     | 40                 | 85.10        | 07:04                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                          | 50                 | 78.80        | 07:51                   |
| [0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5]                                                             | 20                 | 96.10        | 08:06                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0, 21.0]                                                                   | 30                 | 90.15        | 07:19                   |
| [1.0, 2.5, 5.0, 7.5, 9.0, 11.5, 14.0, 16.5]                                                               | 40                 | 84.75        | 06:57                   |
| [4.0, 5.0, 6.0, 7.0, 8.0, 9.0]                                                                            | 50                 | 80.10        | 07:39                   |
| [1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5]                                                                       | 20                 | 95.85        | 08:11                   |
| [3.0, 4.5, 6.0, 7.5, 9.0, 10.5]                                                                           | 30                 | 91.75        | 07:26                   |
| [0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0]                                                                       | 40                 | 84.85        | 06:58                   |
| [2.0, 3.5, 5.0, 6.5, 8.0, 9.5, 11.0]                                                                      | 50                 | 79.25        | 07:31                   |
| [1.0, 2.5, 5.0, 7.5, 10.0, 12.5]                                                                          | 20                 | 94.35        | 08:09                   |
| [3.0, 6.0, 9.0, 12.0, 15.0]                                                                               | 30                 | 90.25        | 07:16                   |
| [1.0, 2.5, 5.0, 7.5, 10.0, 12.5, 15.0]                                                                    | 40                 | 84.40        | 06:54                   |
| [4.0, 6.5, 8.0, 10.5, 13.0]                                                                               | 50                 | 78.70        | 07:49                   |
| [2.0, 4.5, 6.0, 8.0, 10.5, 12.0]                                                                          | 20                 | 95.60        | 08:14                   |
| [3.0, 6.0, 9.0, 12.0, 15.0]                                                                               | 30                 | 90.75        | 07:22                   |
| [2.5, 4.0, 6.0, 8.0, 10.5, 12.0]                                                                          | 40                 | 84.60        | 07:02                   |
| [4.0, 5.0, 6.0, 7.0, 8.0]                                                                                 | 50                 | 79.10        | 07:45                   |
| [0.5, 1.0, 2.0, 3.0, 4.0, 5.0]                                                                            | 20                 | 94.75        | 07:53                   |
| [3.0, 4.5, 6.0, 7.5, 9.0, 10.5]                                                                           | 30                 | 89.95        | 07:18                   |
| [1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5]                                                                       | 40                 | 84.20        | 06:55                   |
| [3.0, 6.0, 9.0, 12.0, 15.0]                                                                               | 50                 | 80.15        | 07:35                   |
| [1.0, 2.5, 5.0, 7.5, 10.0, 12.5]                                                                          | 20                 | 95.40        | 08:05                   |
| [3.0, 6.0, 9.0, 12.0, 15.0, 18.0]                                                                         | 30                 | 89.80        | 07:21                   |
| [0.5, 1.0, 2.0, 3.0, 4.0, 5.0]                                                                            | 40                 | 84.10        | 07:00                   |
| [2.5, 4.5, 6.5, 8.0, 10.0]                                                                                | 50                 | 78.80        | 07:42                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0]                                                                            | 20                 | 96.00        | 08:13                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                          | 30                 | 90.50        | 07:29                   |
| [3.0, 5.0, 7.0, 9.0, 11.0]                                                                                | 40                 | 84.90        | 07:04                   |
| [4.0, 6.0, 8.0, 10.0]                                                                                     | 50                 | 79.25        | 07:37                   |
| [0.0, 1.0, 2.0, 3.0, 4.0, 5.0]                                                                            | 20                 | 95.85        | 08:07                   |
| [1.0, 2.5, 5.0, 7.5, 10.0]                                                                                | 30                 | 90.10        | 07:24                   |
| [2.5, 4.5, 6.5, 8.5, 10.0]                                                                                | 40                 | 83.90        | 07:03                   |
| [1.0, 2.0, 3.0, 4.0, 5.0]                                                                                 | 50                 | 78.90        | 07:43                   |
| [2.0, 3.5, 5.0, 6.5, 8.0, 9.5, 11.0]                                                                      | 20                 | 96.10        | 08:15                   |
| [1.0, 3.0, 5.0, 7.0, 9.0]                                                                                 | 30                 | 89.65        | 07:25                   |
| [4.0, 5.5, 7.0, 8.5, 10.0]                                                                                | 40                 | 84.25        | 07:00                   |
| [2.0, 3.0, 4.0, 5.0, 6.0]                                                                                 | 50                 | 79.50        | 07:39                   |
| [1.5, 3.5, 5.5, 7.5, 9.5, 11.5, 13.5, 15.5]                                                               | 20                 | 94.50        | 07:59                   |
| [2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                          | 30                 | 90.25        | 07:14                   |
| [3.0, 5.0, 7.0, 9.0, 11.0]                                                                                | 40                 | 83.85        | 07:08                   |
| [1.0, 2.5, 5.0, 7.5, 10.0, 12.5]                                                                          | 50                 | 78.75        | 07:47                   |
| [0.0, 2.0, 4.0, 6.0, 8.0, 10.0, 12.0]                                                                     | 20                 | 95.95        | 08:10                   |
| [3.0, 6.0, 9.0, 12.0, 15.0]                                                                               | 30                 | 91.20        | 07:31                   |
| [4.5, 5.5, 6.5, 7.5, 8.5]                                                                                 | 40                 | 84.75        | 07:15                   |
| [1.0, 2.0, 3.0, 4.0, 5.0, 6.0]                                                                            | 50                 | 79.35        | 07:41                   |
| [1.5, 2.5, 3.5, 4.5, 5.5]                                                                                 | 20                 | 96.10        | 08:14                   |
| [3.0, 5.0, 7.0, 9.0, 11.0]                                                                                | 30                 | 90.50        | 07:27                   |
| [1.0, 2.5, 5.0, 7.5, 10.0]                                                                                | 40                 | 84.20        | 07:12                   |
| [2.0, 4.0, 6.0, 8.0, 10.0]                                                                                | 50                 | 78.95        | 07:50                   |
| [1.0, 2.5, 5.0, 7.5, 10.0, 12.5, 15.0]                                                                    | 20                 | 94.85        | 08:09                   |


## Discussion and Conclusion

The various tests conducted using different datasets and `maxNewSynapseCount` values provide critical insights into how different parameters influence the efficiency and accuracy of sequence learning models. Below is a detailed discussion of the results:

### 1. Accuracy vs. MaxNewSynapseCount
Across multiple datasets, we observed that lower values of `maxNewSynapseCount` (20 and 30) generally led to higher accuracy scores compared to higher values (40 and 50). For simpler datasets, such as sequences with smaller or fewer variations, the model achieves near-perfect accuracy with lower `maxNewSynapseCount`. 

For more complex datasets, slightly larger `maxNewSynapseCount` values (around 30) can yield decent accuracy while avoiding a steep drop in performance. This demonstrates the importance of tuning `maxNewSynapseCount` based on the complexity of the dataset to achieve optimal performance.

### 2. Cycles to Stabilize vs. MaxNewSynapseCount
The results show a trend where larger `maxNewSynapseCount` values generally lead to fewer cycles to stabilize. Specifically, for higher values like 40 and 50, the model stabilizes quicker, particularly in complex datasets.

For simpler datasets, lower values (20 and 30) also resulted in fewer cycles, suggesting that complex architectures are unnecessary and may lead to slower learning.

**Conclusion**: For complex datasets, using a higher `maxNewSynapseCount` can stabilize the model faster, whereas for simple datasets, lower values are more efficient both in terms of speed and accuracy.

### 3. Test Duration vs. MaxNewSynapseCount
The test duration closely aligns with the number of cycles required to stabilize the model. Larger `maxNewSynapseCount` values generally resulted in longer test durations, especially for complex datasets. 

For simple datasets, lower `maxNewSynapseCount` values (20) often led to shorter durations and still provided high accuracy scores. In contrast, higher values such as 50 not only took longer but also showed a decrease in accuracy for these simpler datasets.

### 4. Balancing Accuracy, Cycles to Stabilize, and Duration
There is a trade-off between accuracy, cycles to stabilize, and duration. Lower `maxNewSynapseCount` values perform well on simpler datasets with high accuracy and reasonable stabilization times. On the other hand, higher values stabilize faster but often lead to lower accuracy in simpler datasets and longer durations.

#### Optimal Choice:
- For simpler datasets, `maxNewSynapseCount` values around **20-30** are ideal as they provide high accuracy, stabilize in a reasonable number of cycles, and minimize test duration.
- For more complex datasets, `maxNewSynapseCount` values around **40-50** might be necessary to stabilize the model quickly, though at the cost of slightly lower accuracy and longer test duration.
