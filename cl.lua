exports.ox_lib:registerMenu({
    id = 'clothes_takeoff_menu',
    title = 'Clothes',
    position = 'top-right',
    options = {
        {label = 'Hat', args = 'hat'},
        {label = 'Mask', args = 'mask'},
        {label = 'Top', args = 'top'},
        {label = 'Pants', args = 'pants'},
        {label = 'Shoes', args = 'shoes'},
        {label = 'Bag', args = 'bag'},
        {label = 'Armour', args = 'armor'},
        {label = "Glasses", args='glass'},
        {label = 'Bracelet', args = 'bracelets'},
        {label = 'Earrings', args = 'ears'},
        {label = 'Watch', args = 'watch'}
    } 
    }, function(selected, scrollIndex, args)
        TriggerEvent('uiClothes:takeOff', args)
    end
)

RegisterCommand('uiclothes', function()
    exports.ox_lib:showMenu('clothes_takeoff_menu')
end)

exports('cloth', function(data,slot)
   local m = ''
   if (slot.metadata.gender == 'm') then
    m = 'mp_m_freemode_01'
   else
    m = 'mp_f_freemode_01'
   end

   local playerPed = GetPlayerPed(-1)
   local model = GetEntityModel(playerPed)
   local mhash = GetHashKey(m)

   if (mhash == model) then
    exports.ox_inventory:useItem(data, function(data)
        if (data) then
            TriggerEvent('uiClothes:putOnClothes', slot.name, slot.metadata.config, slot.metadata.gender)
        end
    end)
   else
        exports.ox_lib:notify({title = "Error", description = "This clothes don't fit on you", type = "error"})
   end


end)

RegisterKeyMapping('uiclothes', 'Open clothes menu', 'keyboard', 'k')