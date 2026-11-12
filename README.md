# MyRealSpace - Realitat Virtual i Augmentada - Meta Building Blocks

<p align="center">
  <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Logo.png" width="30%">
</p>

**GitHub project page**: [MyRealSpace](https://github.com/XXPabloS/MetaBuildingBlocks)

**Gameplay video**: [Watch it here](LINK_TO_VIDEO)

## Description
**MyRealSpace** is an interior design simulator that lets you furnish a real room with virtual furniture and objects in mixed reality. The user sees their own room, can choose any object they want from the catalog, and places it by pointing the controllers at the floor, wall, or any other surface; they can rotate or scale the object to make it fit the space.

## Building blocks Used

| Building Block | Use in the project |
| --- | --- |
| MR Utility Kit (MRUK) | Provides the model of the real room (floor, walls and other surfaces) and lets the ray find where it hits. |
| Passthrough | Shows the real room through the headset, so the furniture and objects look integrated in the physical space. |
| Controller input (OVRInput) | Trigger, buttons, joystick and grip to place, select, rotate, scale and delete objects. |
| Ray interaction | A ray from the controller or hand is used to point at surfaces and at placed furniture. |
| 6DoF locomotion | The user walks around their real room, no teleport is needed. |


## Requirements
- Meta Quest 3 (recommended, as the experience uses mixed reality and the room model).
- A room that can be scanned by the headset, with enough free space to move around.


## Installation
1. **Download and unzip the [RELEASE FOLDER](https://github.com/XXPabloS/MetaBuildingBlocks/releases).**
2. Install the `.apk` file on your Meta Quest 3.
3. Launch the app in your Quest library.

## Controls

| Action | Controllers |
| --- | --- |
| Place an object | Right index trigger |
| Change object | Button A (with nothing selected) |
| Rotate on the vertical axis "Y" | Right joystick, left / right |
| Rotate on the "X" and "Z" axes | Hold grip + right joystick (up / down for X, left / right for Z) |
| Scale | Right joystick, up / down |
| Deselect the object (confirm its position) | Right index trigger |
| Delete the selected object | Button B |
     
## How to Use

<table>
  <tr>
    <td width="30%">
      <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_1.png" width="100%">
    </td>
    <td>
      <b>1. Scan your room</b><br>
       Walk trough the room you want to decorate so the application can identify walls, floor, ceiling and other collisions. 
    </td>
  </tr>

  <tr>
    <td width="30%">
      <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_2.png" width="100%">
    </td>
    <td>
      <b>2. Choose a furniture</b><br>
       Open the menu and select the furniture you want to place.
    </td>
  </tr>

  <tr>
    <td width="30%">
      <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_3.png" width="100%">
    </td>
    <td>
      <b>3. Place the furniture</b><br>
       Point where u want the furniture to spawn and press the <b>index button</b>. When the furniture appears press <b>index button</b> to confirm or <b>B button</b> to cancel.
    </td>
  </tr>

  <tr>
    <td width="30%">
      <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_4.png" width="100%">
    </td>
    <td>
      <b>4. Adjust the furniture</b><br>
       Before pressing confirm the placement of a furniture u can adjust their position by pointing to other place, modify scale and rotation with the <b>Joystick</b>.
    </td>
  </tr>

  <tr>
    <td width="30%">
      <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_5.png" width="100%">
    </td>
    <td>
      <b>5. Modify placed furniture</b><br>
      By pointing to a placed furniture press <b>index button</b> to select, now you can modify position, scale and rotation as mentioned on raw 4. Or you can also delete it by pressing <b>B button</b>
    </td>
  </tr>
</table>

<p align="center">
  <img src="Activity1_AR_Foundation/Assets/CITM/Tutorials/Tutorial_6.png" width="30%">
  <br>
  <strong>THAT'S ALL! HAVE FUN!</strong>
</p>
     
## Credits
**All contributors working on this project**:

_Ana Alcaraz_ « **Github**: [Audra0000](https://github.com/Audra0000)

_Arthur Cordoba_ « **Github**: [000Arthur](https://github.com/000Arthur)

_Haosheng Li_ « **Github**: [HaosLii](https://github.com/HaosLii)

_Pablo Sanjose_ « **Github**: [XXPabloS](https://github.com/XXPabloS)

_Joel Vicente_ « **Github**: [Jowy02](https://github.com/Jowy02)

**3D models**: Low-Poly Office Set #1 [+140 Models] by VNB, from the Unity Asset Store.
