# Setup The Project In Your Local Environment

1. Install and setup
  - `Microsoft SQL Server Express LocalDB`,
  - `Microsoft Visual Studio 2019/2022`,
  - `Web browsers with JavaScript enabled`,
  - `.NET Framework latest`,
   in your local environment.
2. Download the zip file or clone the repository to a folder of your preference in your computer.
3. Extract the project folder if you have downloaded the zip file (ignore this if you have cloned the repository).
4. Go to the project folder and open the `FidenzProjectMVC.sln` in `Visual Studio`.
5. Right click on `FidenzProjectMVC` in `Solution Explorer` and select `Build`.
6. When the building is complete start the application with `https`.


# Testing Credentials

Admin user
  - Email : **admin@example.com**
  - Password : **AdminPassword123!**
    
User user
  - Email : **user@example.com**
  - Password : **UserPassword123!**

# Using The Application

1. To test the API go to `https://localhost:<YourPort>/Swagger/index.html`.
2. Generate the token from /api/API/login using **Admin** credentials.
3. Paste the token in **Authorize** provided by Swagger and API endpoints are now accessible.
   (**If you enter User credentials and the generated token from that will not give you access to the API endpoints**).
4. Go to `https://localhost:<YourPort>` to login through the view provided in order to view **AdminDashboard** and **UserDashboard**.
5. When you login through the view a token is generated and stored as a `cookie`.
6. You can use the same token **step 3**.
