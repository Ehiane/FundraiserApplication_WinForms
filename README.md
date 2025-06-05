# WSU Fundraiser Management System 🎓💰

A mock C#-based Windows Forms application designed to simulate a comprehensive fundraising ecosystem for a university environment. This project serves as a practical demonstration of Object-Oriented Programming (OOP), software design patterns, and UI development with WinForms.

---

## 📖 About The Project

This application provides a centralized platform for students, donors, and administrators to manage university clubs, fundraising projects, scholarships, and donations. Built entirely in C# with data persistence handled through XML, it showcases core software engineering principles in a desktop application format. The user-friendly interface allows for seamless interaction, from creating a club to processing donations and awarding scholarships.

This project was developed to model real-world software engineering practices, emphasizing a clean architecture and robust functionality for academic and portfolio purposes.

### ✨ Key Features

* **User Authentication & Roles**: Secure login system for different user types (**Student**, **Donor**, **Guest**, and **Administrator**) with distinct permissions and views.
* **Club & Project Management**: Students can create and manage their own clubs, create fundraising projects associated with those clubs, and track progress.
* **Scholarship System**: A complete workflow for creating scholarship opportunities, allowing students to apply, and for administrators to review applications and award funds.
* **Donation & Transaction Handling**: Donors can easily contribute to specific clubs or projects. The system logs all transactions, providing summaries and tracking financial data.
* **Data Persistence**: All application data, including users, clubs, and transactions, is saved and loaded from a local `WSUFundraiserDatabase.xml` file, simulating a database.
* **User Profile Personalization**: Users can upload and manage their profile pictures, which are stored locally.
* **Comprehensive Testing**: Includes a dedicated test suite using **NUnit** to validate the application's logic, covering normal operations, boundary conditions, and exceptional scenarios.

---

### 🧰 Technologies & Principles

* **Backend**: C# (.NET Framework)
* **Frontend**: Windows Forms (WinForms)
* **Data Storage**: XML
* **Testing**: NUnit
* **Core Principles**: Object-Oriented Programming (OOP)
* **Architectural Patterns**: Utilizes foundational patterns like **Singleton** (for the `Fundraiser` engine), **Factory** (for user creation), and a structure inspired by **Model-View-Controller (MVC)**.

---

## 🚀 Getting Started

Follow these instructions to get a local copy of the project up and running on your machine.

### Prerequisites

* **Visual Studio** 2019 or later
* **.NET Framework** 4.7.2 or later

### Installation & Setup

1.  **Clone the repository:**
    ```sh
    git clone [https://github.com/Ehiane/FundraiserApplication_WinForms.git](https://github.com/Ehiane/FundraiserApplication_WinForms.git)
    ```
2.  **Navigate to the project directory:**
    ```sh
    cd FundraiserApplication_WinForms
    ```
3.  **Open the solution file** (`FundraiserApplication_WinForms.sln`) in Visual Studio.
4.  **Build the solution** to restore dependencies (Right-click on the solution in Solution Explorer -> `Build Solution`).
5.  **Run the application** by setting `FundraiserApp` as the startup project and pressing `F5` or the "Start" button. The main entry point is `FundraiserApp.cs`.

---

### 💾 Data Storage

* **Database**: The application uses a single XML file, `WSUFundraiserDatabase.xml`, which acts as the database. It will be created automatically in a fixed local path (`C:\Users\Public\WSUFundraiserDatabase.xml`) upon first run if it doesn't exist.
* **User Images**: Profile pictures are stored in the `FundraiserApplication_WinForms/Images/` directory within the project structure.

---

## 🧪 Testing

The solution includes a `Fundraiser_NUnit_Tests` project that contains a suite of unit tests built with NUnit. These tests cover the core logic of the `WSUFundraiserEngine` to ensure reliability and correctness.

To run the tests:
1.  Open the **Test Explorer** in Visual Studio (`Test` > `Test Explorer`).
2.  Build the solution to discover the tests.
3.  Click **Run All Tests**.
