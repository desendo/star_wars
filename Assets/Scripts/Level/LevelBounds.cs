using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Level
{
    public sealed class LevelBounds : MonoBehaviour
    {
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;
        [SerializeField] private Transform _downBorder;
        [SerializeField] private Transform _topBorder;

        private float _topY;
        private float _bottomY;
        private float _leftX;
        private float _rightX;
        private float _width;
        private float _height;

        private readonly float _enlargeZone = 1f;
        private void Awake()
        {
            FitBordersToScreen(_enlargeZone);
            InitParameters();
        }

        public Vector2 GetRandomPointInBorders()
        {
            var x = UnityEngine.Random.Range(_leftX, _rightX);
            var y = UnityEngine.Random.Range(_bottomY, _topY);
            return new Vector2(x, y);
        }

        private void FitBordersToScreen(float factor)
        {
            var cam = Camera.main;
            _leftBorder.position = cam.ViewportToWorldPoint(new Vector2(0, 0.5f)) - new Vector3(factor, 0);
            _rightBorder.position = cam.ViewportToWorldPoint(new Vector2(1, 0.5f)) + new Vector3(factor, 0);
            _downBorder.position = cam.ViewportToWorldPoint(new Vector2(0.5f, 0)) - new Vector3(0, factor);
            _topBorder.position = cam.ViewportToWorldPoint(new Vector2(0.5f, 1)) + new Vector3(0, factor);;

        }

        private void InitParameters()
        {
            _leftX = _leftBorder.position.x;
            _rightX = _rightBorder.position.x;
            _bottomY = _downBorder.position.y;
            _topY = _topBorder.position.y;
            _width = _rightX - _leftX;
            _height = _topY - _bottomY;
        }

        public bool InBounds(Vector3 position)
        {
            var positionX = position.x;
            var positionY = position.y;
            return positionX > _leftX
                   && positionX < _rightX
                   && positionY > _bottomY
                   && positionY < _topY;
        }

        public Vector3 Warp(Vector3 pos)
        {
            while (!InBounds(pos))
            {
                pos = TranslateCoordinates(pos);
            }
            return pos;
        }
        public void Warp(Transform tr)
        {
            tr.position = Warp(tr.position);
        }
        private Vector3 TranslateCoordinates(Vector3 position)
        {
            var positionX = position.x;
            var positionY = position.y;

            if (positionX < _leftX)
                position.x += _width;
            else if (positionX > _rightX)
                position.x -= _width;
            if (positionY < _bottomY)
                position.y += _height;
            else if (positionY > _topY)
                position.y -= _height;

            return position;
        }
    }
}