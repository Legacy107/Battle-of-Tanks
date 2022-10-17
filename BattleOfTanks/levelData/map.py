# Script to format tile map from https://blurymind.github.io/tilemap-editor/#
import json

tileMap = {
    '¬': 'Steel',
    '«': 'Wall',
    'ª': 'Sand',
}

with open('tilemap-editor.json') as fjson:
    data = json.load(fjson)
    for i in range(len(data['maps'])):
        tiles = data['maps'][f'Map{i}']['layers'][0]['tiles']

        with open(f'level{i}.txt', 'w') as f:
            tile_output = ''
            tile_count = 0
            for key, value in tiles.items():
                key = key.split('-')
                key = [int(key[0]), int(key[1])]

                if key[0] >= 29 or key[1] >= 30:
                    continue

                id = key[1] * 29 + key[0]
                tile = tileMap.get(value['tileSymbol'])
                
                if tile:
                    tile_output += (f'{id}\n{tile}\n')
                    tile_count += 1
            
            f.write(f'380\n660\n1\n30\n30\n{tile_count}\n{tile_output}')
