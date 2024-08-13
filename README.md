# rbxpack

rbxpack is a CLI tool that makes it simple to handle developing and distributing a game across launchers of old Roblox (2006-2012) clients. Supported launchers are Novetus and Only Retro Roblox Here.

## Features

Basically, rbxpack does a few different things:

### Init project

Run `rbxpack init` to set up the `rbxpack.json` config file, containg metadata about the project.

- ProjectName: A short, (preferably all lowercase alphanumeric) name of the project for internal use (name of the assets folder).
- FriendlyName: The project name, used in ORRH asset pack metadata.
- Description: The project description, used in ORRH asset pack metadata and Novetus map metadata.
- Author: The project author, used in ORRH asset pack metadata.
- Version: The project version, used in ORRH asset pack metadata.
- Clients: An array of clients the project works for, used in ORRH asset pack metadata and which clients to link to.[^orrhclients]
- ProjectRbxl: The name of the rbxl file. For example, HappyHome.rbxl.
- Links: An array of objects containing data about launchers to link to:
    - ClientsDir: The directory containing clients in the launcher.[^orrhclientsdir]
    - MapsDir: The directory containing maps in the launcher.

### Link to launchers

rbxpack allows you to develop your game outside of a specific launcher, and even develop your game in multiple launchers simultaneously. Store a project folder containing your game and assets anywhere, and link it to a launcher's clients and maps directories with `rbxpack link add`. rbxpack will then:

- [Symlink](https://en.wikipedia.org/wiki/Symbolic_link) your project folder to the launcher's maps folder
- For every client listed in the config, rbxpack will symlink your project's assets folder to under the client's content folder, using the project's configured internal name + an identifier suffix. For example, the "bloxparty" project can access `assets/grass.png` in Roblox Studio with `rbxasset://bloxparty-rbxpack/grass.png`. 

### Bundle project for distribution

Running `rbxpack build` will repack your game and assets for Novetus and ORRH's unique directory structures, writing to `build/novetus` and `build/orrh` directories in your project.

- Rewrites asset URLs and asset file names where necessary
- Minifies the RBXL file's XML and Lua contents to reduce size (TODO!)
- Generate ORRH asset pack metadata and Novetus map description metadata
- Compresses the RBXL to .rbxl.bz2 (for Novetus) and .rbxl.gz (for ORRH)
- Compresses the generated assets folder to a .zip for distribution

[^orrhclients] This uses ORRH's syntax:
- `CLIENT`: Only enables for CLIENT. For example, 2012L.
- `!CLIENT`: Disables for CLIENT. For example, !2012L.
- `*`: Enable for all clients.

[^orrhclientsdir] rbxpack is trying to get to the `content` directory of the client. ORRH stores its actual clients in subfolders of the Clients directory: `./data/clients/CLIENT/Player`, unlike Novetus. rbxpack will detect this, so just set the clients dir to ORRH's general clients directory and it will be fine.