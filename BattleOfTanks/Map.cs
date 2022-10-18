using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Map
    {
        private List<IMapTile> _tiles;
        private Point2D _playerSpawn;
        private List<Point2D> _enemySpawns;
        private Base _base;

        public Map()
            : this("./levelData/" + GameConfig.DEFAULT_LEVEL)
        {
        }

        public Map(string filename)
        {
            _base = new Base(0, 0);

            // Load a 30 x 29 map from a file
            StreamReader reader = new StreamReader(filename);

            // Read spawnpoints for player and enemy
            _playerSpawn = reader.ReadPoint2D();

            int enemySpawnCount = reader.ReadInteger();
            _enemySpawns = new List<Point2D>();
            for (int i = 0; i < enemySpawnCount; i++)
                _enemySpawns.Add(reader.ReadPoint2D());

            // Read tiles map
            int count = reader.ReadInteger();
            string? kind;
            IMapTile? tile = null;
            _tiles = new List<IMapTile>();

            for (int i = 0; i < count; i++)
            {
                // index of a tile starting from top left
                int idx = reader.ReadInteger();
                kind = reader.ReadLine();
                Point2D coord = GetCoordFromIdx(idx);
                double x = coord.X;
                double y = coord.Y;

                switch (kind)
                {
                    case "Wall":
                        tile = new Wall(x, y);
                        break;
                    case "Sand":
                        tile = new Sand(x, y);
                        break;
                    case "Steel":
                        tile = new MetalWall(x, y);
                        break;
                    case "Base":
                        _base = new Base(x, y);
                        tile = _base;
                        break;
                    default:
                        throw new InvalidDataException(string.Format(
                            "Unknown tile kind: {0}",
                            kind
                        ));
                }

                if (tile != null)
                    _tiles.Add(tile);
            }

        }

        private Point2D GetCoordFromIdx(int idx)
        {
            int row = idx % GameConfig.MAP_WIDTH;
            int col = idx / GameConfig.MAP_WIDTH;

            return SplashKit.PointAt(
                GameConfig.TILE_SIZE * row,
                GameConfig.TILE_SIZE * col
            );
        }

        public void Draw(Window window)
        {
            foreach (IMapTile mapTile in _tiles)
                mapTile.Draw(window);
        }

        public void CheckCollision(PhysicalObject obj)
        {
            foreach (IMapTile mapTile in _tiles)
            {
                if (mapTile is GameObject tileObject)
                    obj.IsCollided(tileObject);

                mapTile.ObjectCollision(obj);
            }
        }

        public void CheckRemoval()
        {
            foreach (IMapTile mapTile in _tiles.Reverse<IMapTile>())
                if (mapTile is GameObject tileObject && tileObject.NeedRemoval)
                    _tiles.Remove(mapTile);
        }

        public Point2D PlayerSpawn
        {
            get
            {
                return _playerSpawn;
            }
        }

        // Return a random spawnpoint for enemy
        public Point2D EnemySpawn
        {
            get
            {
                Random random = new Random();
                return _enemySpawns[
                    random.Next(_enemySpawns.Count)
                ];
            }
        }

        public Base Base
        {
            get
            {
                return _base;
            }
        }
    }
}

