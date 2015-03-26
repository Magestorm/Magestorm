using System;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace Helper
{
    public class GridBlockCollection : ListCollection<GridBlock>
    {
        public GridBlockCollection(Boolean isBase)
        {
            if (isBase) Add(new GridBlock(0, 0, 0));
        }

        public GridBlock GetBlockByLocation(Single x, Single y)
        {
			return this[(Int32)System.Math.Floor(x / 64f) + (Int32)System.Math.Floor(y / 64f) * 128 + 1];
        }

        public GridBlockCollection GetBlocksNearBoundingBox(OrientedBoundingBox boundingBox)
        {
            return new GridBlockCollection(false)
                       {
                           GetBlockByLocation(boundingBox.Origin.X,boundingBox.Origin.Y),
                           GetBlockByLocation(boundingBox.Corners[0].X, boundingBox.Corners[0].Y),
                           GetBlockByLocation(boundingBox.Corners[1].X, boundingBox.Corners[1].Y),
                           GetBlockByLocation(boundingBox.Corners[2].X, boundingBox.Corners[2].Y),
                           GetBlockByLocation(boundingBox.Corners[3].X, boundingBox.Corners[3].Y)
                       };
        }

        public GridBlockCollection GetBlocksInLine(Vector3 startPoint, Vector3 endPoint)
        {
            GridBlockCollection gridBlockCollection = new GridBlockCollection(false);

            Vector3 currentPoint = startPoint;
            Vector3 direction = Vector3.Normalize(endPoint - startPoint);
            Single originalDistance = Vector3.Distance(currentPoint, endPoint);

            while (Vector3.Distance(startPoint, currentPoint) < originalDistance)
            {
                GridBlock block = GetBlockByLocation(currentPoint.X, currentPoint.Y);

                if (block != null && !gridBlockCollection.Contains(block)) gridBlockCollection.Add(block);

                currentPoint += direction;
            }

            for (Int32 i = gridBlockCollection.Count - 1; i >= 0; i--)
            {
                GridBlock block = gridBlockCollection[i];
                if (block == null) continue;

                if (block.LowBox == null || block.MidBox == null || block.HighBox == null) continue;
                if (block.LowBox.LineInBox(startPoint, endPoint) || block.MidBox.LineInBox(startPoint, endPoint) || block.HighBox.LineInBox(startPoint, endPoint)) continue;

                gridBlockCollection.RemoveAt(i);
             }

            return gridBlockCollection;
        }

        public GridBlockCollection GetBlocksAroundLine(Vector3 startPoint, Vector3 endPoint)
        {
            GridBlockCollection gridBlockCollection = new GridBlockCollection(false);

            Vector3 currentPoint = startPoint;
            Vector3 direction = Vector3.Normalize(endPoint - startPoint);
            Single originalDistance = Vector3.Distance(currentPoint, endPoint);

            while (Vector3.Distance(startPoint, currentPoint) < originalDistance)
            {
                GridBlock block = GetBlockByLocation(currentPoint.X, currentPoint.Y);

                if (block != null && !gridBlockCollection.Contains(block)) gridBlockCollection.Add(block);

                currentPoint += direction;
            }

            for (Int32 i = gridBlockCollection.Count - 1; i >= 0; i--)
            {
                GridBlock block = gridBlockCollection[i];
                if (block == null) continue;

                if (block.ContainerBox == null) continue;
                if (block.ContainerBox.LineInBox(startPoint, endPoint)) continue;

                gridBlockCollection.RemoveAt(i);
            }

            return gridBlockCollection;
        }

        public GridBlock GetHighestGravityBlock(OrientedBoundingBox boundingBox)
        {
            GridBlockCollection gridBlockCollection = GetBlocksNearBoundingBox(boundingBox);

            for (Int32 i = gridBlockCollection.Count - 1; i > 0; i--)
            {
                if (gridBlockCollection[i].LowBoxTopZ < gridBlockCollection[i-1].LowBoxTopZ)
                {
                    gridBlockCollection.RemoveAt(i);
                }
                else
                {
                    gridBlockCollection.RemoveAt(i-1);
                }
            }

            return gridBlockCollection[0];
        }
    }
}
