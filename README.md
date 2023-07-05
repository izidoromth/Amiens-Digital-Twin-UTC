# Digital Twin for Building a Climate-Resilient City: A Software Prototype Approach for Amiens

This project is a digital twin prototype for the city of Amiens, developed to assist policy makers in the management and prevention of climate risks. It was created as part of the Atelier Projet 00 at Université de Technologie de Compiègne during the Spring semester of 2023.

## Overview

The digital twin prototype provides a platform for simulating and analyzing climate risks specific to the city of Amiens. It offers various features and functionalities to aid policy makers in making informed decisions related to climate risk management and prevention.

## Installation

To run the prototype, follow the instructions below:

### Prerequisites

- PostgresSQL connection with the provided database dump file named "amiens_digital_twin_03072023.sql".
- All the .fbx files must be inside the Resources folder.

### Steps

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/izidoromth/Amiens-Digital-Twin-UTC.git
   ```

2. Set up the PostgresSQL connection:

    * Ensure you have PostgresSQL installed and running on your machine.

    * Create a new database or use an existing one to import the data.

    * Open a command prompt or terminal and navigate to the project's directory.

    * Run the following command to import the database dump file:
    
    ```
    psql -U <username> -d <database_name> -f path/to/amiens_digital_twin_03072023.sql
    ```
    
3. Download [Resources.zip](https://drive.google.com/file/d/1DbIOXX1SOsgLe7IETHgEP0r2FNcgQq-z/view?usp=sharing) and extract all the files in the "Assets > Resources" folder.

### Usage

    Launch the digital twin prototype application.

    Explore the various features and functionalities provided by the prototype to analyze and manage climate risks in the city of Amiens.

### Contributing

Contributions to the project are welcome. If you encounter any issues or have suggestions for improvements, please submit a pull request.

### Acknowledgments

We would like to express our gratitude to Amines city and Université de Technologie de Compiègne for their support and contributions to this project.

For any further questions or inquiries, please contact the project team at matheusizidoro07@gmail.com or gilles.morel@utc.fr.

Thank you for using the Digital Twin Prototype for Amiens!
