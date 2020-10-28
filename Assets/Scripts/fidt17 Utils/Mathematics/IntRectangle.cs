using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fidt17.Utils {
    public class IntRectangle {
        
        public int Width => _end.x - _start.x;
        public int Height => _end.y - _start.y;

        public Vector2Int Start => _start;
        public Vector2Int End   => _end;

        private Vector2Int _start;
        private Vector2Int _end;

        public IntRectangle(Vector2Int start, Vector2Int end) {
            ArrangeCoordinates(start, end);
        }

        public bool CompareTo(IntRectangle otherRect) => this.Start == otherRect.Start && this.End == otherRect.End;

        public bool Contains(Vector2Int position) => position.x >= _start.x && position.x <= _end.x && position.y >= _start.y && position.y <= _end.y;

        public List<Vector2Int> GetPositions() {
            List<Vector2Int> list = new List<Vector2Int>();
            for (int x = _start.x; x <= _end.x; x++) {
                for (int y = _start.y; y <= _end.y; y++) {
                    list.Add(new Vector2Int(x, y));
                }
            }
            return list;
        }

        private void ArrangeCoordinates(Vector2Int start, Vector2Int end) {
            _start = new Vector2Int();
            _end = new Vector2Int();

            if (start.x <= end.x) {
                _start.x = start.x;
                _end.x = end.x;
            } else {
                _start.x = end.x;
                _end.x = start.x;
            }

            if (start.y <= end.y) {
                _start.y = start.y;
                _end.y = end.y;
            } else {
                _start.y = end.y;
                _end.y = start.y;
            }
        }
    }
}