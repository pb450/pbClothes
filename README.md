
# pbClothes
Simple script allowing to take off, exchange, and put clothes on your MP model in FiveM ([See how it works](https://youtu.be/tJcbnBoIIVs "See how it works"))
## Installation
1. Install required depedencies: [ox_inventory](https://github.com/overextended/ox_inventory "ox_inventory") and [ox_lib](https://github.com/overextended/ox_lib "ox_lib")
2. Open zip downloaded from relases and copy pbClothes folder to resources folder
3. Content of images folder copy to web/build/images in ox_inventory folder
4. Add text from toItems.txt to your data/items.lua file
5. Add ensure/start command to your cfg file

## Configuration
1. In defaults.json you can set default body parts that will appear after taking your cloth off (Indexes can be found [here](https://wiki.rage.mp/index.php?title=Clothes "here"))
2. In last line of cl.lua you can change default key that will be used to open menu (More [here](https://docs.fivem.net/natives/?_0xD7664FD1 "here"))

## Usage
At game press selected key (Default is K) to open menu. Navigate using down/up arrow keys. Press enter at selected index to remove cloth. Press Esc key to cancel and close menu
In order to put cloth on, "use it" in ox_inventory either by Use function in F2 menu or by 1-5 shortcuts. If cloth assigned model is  different that your current one you will be notifed and cloth not removed

**Important**: Script itself does not save clothes to any database (Only changes drawables and props). Because of that this script emits event `pbClothes:onDrawableChange` when drawable is changed and `pbClothes:onPropChange` when any prop changes. Events does not have any arguments 
