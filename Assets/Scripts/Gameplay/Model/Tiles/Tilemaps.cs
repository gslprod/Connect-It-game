using ConnectIt.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ConnectIt.Model
{
    public class Tilemaps
    {
        private TilemapLayerSet[] _maps;

        public Tilemaps(TilemapLayerSet[] maps)
        {
            Validate(maps);

            _maps = maps;
        }

        public void GetTileAtPosition(Vector3 position)
        {

        }

        private void Validate(TilemapLayerSet[] maps)
        {
            int duplicatesGroupsCount = 
                _maps.GroupBy(set => set.Layer)
                .Where(group => group.Count() > 1)
                .Count();

            Assert.That(duplicatesGroupsCount == 0);
        }
    }

    public class TilemapLayerSet
    {
        public Tilemap Map { get; set; }
        public TileLayers Layer { get; set; }
    }
}
