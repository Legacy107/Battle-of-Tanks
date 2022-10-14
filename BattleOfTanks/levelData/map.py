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
            f.write('380\n660\n1\n30\n30\n')
            f.write(f'{len(tiles)}\n')
            for key, value in tiles.items():
                key = key.split('-')
                id = int(key[1]) * 29 + int(key[0])
                tile = tileMap[value['tileSymbol']]
                f.write(f'{id}\n{tile}\n')
