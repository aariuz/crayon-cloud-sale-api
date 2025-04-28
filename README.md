# Crayon-Cloud-Sale-API

A simple api that is part of an exercise for Crayon. This solution handles simple requests from customers (may it be either end customers or resellers).
Requests such as creating customers, accounts, listing and purchasing software etc.

---

## Table of Contents

- [Project Description](#project-description)
- [Installation](#installation)
- [Usage](#usage)

---

## Project Description

The solution is split into several smaller projects. The main project is the API. Under Integrations you will find the connection to the Crayon DB, as well as
the connection to the CCP. Access to these integrations are done through read and write facades located in projects with matching names. The Common project is
a little bit thin, only currently containing some objects that are being used and referenced in both the API and integrations / read and write facades etc.
There is also two small test projects, one for integration tests and one for unit tests.

---

## Installation

If you are running Visual Studio 2022 with LocalDB already installed, you should be able to build and run the project straight away.

### Prerequisites

- .NET 8.0 (or later)

### Steps to Install

1. Clone this repository to your local machine

2. Navigate into the project directory

3. Build the project

4. (Optional) If you want to have the data in the local db presist, you need to stop the Crayon.mdf from being copied upon every build.

---

## Usage

You can use the swagger UI to create a customer and then login. At this point, you will get a JWT as response. Use for example Postman to call endpoints with
an Authorization header with the token as Bearer (and expected body in Json).

![image](https://github.com/user-attachments/assets/66b8bf72-8abc-4ebe-80d6-e5e7dcd8931f)

