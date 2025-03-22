# Task Mangement System

A Task Mangemenet System for manage employees tasks by Admin

## Workflow For The Task Life Cycle

* 1 Admin creates a task and assigns it to an employee → Task is in Pending state.
* 2 Employee marks the task as Completed → Status changes to Completed.
* 3 Admin reviews the task:
  * 3.1 If satisfied → Approves it (Final State: Approved)
  * 3.2 If extra work is needed → Marks it as Not Approved
* 4 Employee sees the "Not Approved" task and works on it again then marks it as Approved → Task is Approved (Final State).

## Database Diagram
![databaseDiagram](https://github.com/user-attachments/assets/eb173b08-e6f3-4ce4-b4d6-c21a5ea1e37b)

## Executing System

* First put your SQL Server Name in appsettings.json file in the connection string
```
"DefaultConnectionString": "Data Source=[YOUR_SQL_SERVER];..."

```
* Run update database command to seed the migration using command line
```
update-database

```
* A default Admin User seeded with credentials
```
email: admin@gmail.com & password: Admin@123

```
