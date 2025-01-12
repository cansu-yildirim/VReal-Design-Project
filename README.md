# VReal Design Project

## Demo

[Watch the demo]()

## Project Report

[View the project report]()

### Setup Instructions

## General Notes

When running the project with a VR headset, uncomment the lines in the `Initiator` script.

For simulation mode, leave the script unchanged.

## Server Setup

Open Unity Hub.

Select Open File and navigate to the `VRealServer` folder within the project directory.

Drag the `Main` scene into the Unity scene workspace.

Delete the default `Untitled` scene.

## Client Setup

Open Unity Hub.

Select Open File and navigate to the `VRealClient` folder within the project directory.

Drag the `Main`, `RoomDrawing`, and `VReal` scenes into the Unity scene workspace.

Delete the default `Untitled` scene.

Set the `RoomDrawing` and `VReal` scenes to the "Unload" state.

## Unity Preferences

Navigate to Edit -> Preferences -> VIU and ensure the following settings:

Initialize on Startup: Unchecked

Simulator: Checked

## Notes and Warnings

* #### Warnings:

Headset not connected warnings can be ignored.

EventSystem warnings are also ignorable.

* #### Exceptions:

A known exception occurs when transitioning to the `VReal` scene due to the sample scene. This can be safely ignored.

* #### Important: If using a single machine for development, click on the server application once after completing Google Sign-In to avoid runtime errors.

### Additional Guidance

For detailed guidance on working with the `VReal` scene, watch this video.

#### Database Example

The following code demonstrates how to use the database:
```
NpgsqlConnection conn = ConnectionManager.getConnection();
conn.Open();
User user = new User(1, "asd@asd");
UsersController uc = new UsersController();
uc.insertUser(conn, user);
```
Ensure the Npgsql plugin file is added under the Assets folder in your Unity project.

For any questions or issues, feel free to open a discussion or submit an issue in this repository.

