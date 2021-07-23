# sbox-nightsky
Custom shader procedural night sky.

Since there's no shader support yet and it's all pretty much hacked together, this is more of a tech demo and a proof of concept for the time being!

![Sky dome](https://i.imgur.com/VoyBk4r.jpg)

# Things to note
Currently the skybox "sticks" to the player within the 32768 radius where the player is always in the middle of this. Whatever object the shader is applied to it will stick to it relative to the camera(this is all handled within the vertex shader)

Currently there's an issue with rendering to a specific render pass, that being said the skybox isn't actually being rendered to the same pass as the the skybox.

There is no property serialization yet. I still need to reverse engineer semantic annotations or at least wait for proper support. This means all the properties of the shader are currently hard coded
