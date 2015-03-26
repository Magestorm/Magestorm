using System;
using System.Linq;
using SharpDX;

namespace Helper.Math
{
    public class OrientedBoundingBox
    {
        public BoundingBox AxisBoundingBox;
        public Vector3 Extents;
        public Matrix InvertedRotationMatrix;

        public Vector3 Location;
        public Vector3 MaxLocation;
        public Vector3 Origin;

        public Single Rotation;
        public Matrix RotationMatrix;
        public Vector3 Size;

        public Boolean IsPivotRotation;

        public Vector3[] ObjectSpaceCorners
        {
            get; private set;
        }

        public Vector3[] Corners
        {
            get; private set;
        }

        public BoundingSphere ExtentSphere
        {
            get; private set;
        }

        public Boolean IsBelowDeathZ
        {
            get
            {
                return Location.Z <= -450f;
            }
        }

        /* A Line based Oriented Bounding Box with pivot rotation. */
        public OrientedBoundingBox(Vector3 point1, Vector3 point2, Vector3 size)
        {
            IsPivotRotation = true;
            Location = point1;
			MaxLocation = new Vector3(point1.X, point1.Y + (Single)System.Math.Round(Vector3.Distance(point1, point2), MidpointRounding.AwayFromZero), point1.Z + size.Z);
            Size = new Vector3((MaxLocation.X - Location.X), (MaxLocation.Y - Location.Y), MaxLocation.Z - Location.Z);

            Extents = Size * 0.5f;
            Origin = Location + Extents;

			Rotation = (Single)System.Math.Atan2(point2.Y - Location.Y, point2.X - Location.X) - (Single)System.Math.Atan2(MaxLocation.Y - Location.Y, MaxLocation.X - Location.X);
            RotationMatrix = MathHelper.CreateMatrixFromAxisAngle(new Vector3(0f, 0f, 1f), Rotation);

            AxisBoundingBox = new BoundingBox(Location, MaxLocation);
            ObjectSpaceCorners = AxisBoundingBox.GetCorners();
            Corners = AxisBoundingBox.GetCorners();

            Single radius = 0;

            for (Byte i = 0; i < 8; i++)
            {
                Single fDist = Vector3.Distance(Origin, Corners[i]);
                if (fDist > radius) radius = fDist;
            }

            ExtentSphere = new BoundingSphere(Origin, radius);

            Rotate();
        }

        public OrientedBoundingBox(Vector3 location, Vector3 size, Single rotation)
        {
            IsPivotRotation = false;

            Location = location;
            Size = size;
            Rotation = rotation;
            MaxLocation = new Vector3(Size.X + Location.X, Size.Y + Location.Y, Size.Z + Location.Z);
            RotationMatrix = MathHelper.CreateMatrixFromAxisAngle(new Vector3(0f, 0f, 1f), rotation);

            Extents = (MaxLocation - Location) * 0.5f;
            Origin = Location + Extents;

            AxisBoundingBox = new BoundingBox(Location, MaxLocation);
            ObjectSpaceCorners = AxisBoundingBox.GetCorners();
            Corners = AxisBoundingBox.GetCorners();

            Single radius = 0;

            for (Byte i = 0; i < 8; i++)
            {
                Single fDist = Vector3.Distance(Origin, Corners[i]);
                if (fDist > radius) radius = fDist;
            }

            ExtentSphere = new BoundingSphere(Origin, radius);

            Rotate();
        }

        public void Move(Vector3 location)
        {
            Location = location;
            MaxLocation = new Vector3(Size.X + Location.X, Size.Y + Location.Y, Size.Z + Location.Z);
            Origin = Location + Extents;

            AxisBoundingBox = new BoundingBox(Location, MaxLocation);
            ObjectSpaceCorners = AxisBoundingBox.GetCorners();
            Corners = AxisBoundingBox.GetCorners();

            if (Rotation > 0.0f) Rotate();

            ExtentSphere = new BoundingSphere(Origin, ExtentSphere.Radius);
        }

        public void MoveAndResize(Vector3 location, Vector3 size)
        {
            Location = location;
            MaxLocation = new Vector3(Size.X + Location.X, Size.Y + Location.Y, Size.Z + Location.Z);
            Size = size;
            Extents = (MaxLocation - Location) * 0.5f;
            Origin = Location + Extents;

            AxisBoundingBox = new BoundingBox(Location, MaxLocation);
            ObjectSpaceCorners = AxisBoundingBox.GetCorners();
            Corners = AxisBoundingBox.GetCorners();

            if (Rotation > 0.0f) Rotate();

            ExtentSphere = new BoundingSphere(Origin, ExtentSphere.Radius);
        }

        public void Rotate()
        {
            InvertedRotationMatrix = Matrix.Invert(RotationMatrix);

            for (Byte i = 0; i < 8; i++)
            {
                Vector3 diffVect = ObjectSpaceCorners[i] - (IsPivotRotation ? Location : Origin);
                Vector3 rotatedVect = (Vector3)Vector3.Transform(diffVect, RotationMatrix) + (IsPivotRotation ? Location : Origin);
                Corners[i] = rotatedVect;
            }

            if (IsPivotRotation)
            {
                Origin = Corners[6] - ((Corners[6] - Corners[0]) * 0.5f);
                MaxLocation = Corners[0];

                ExtentSphere = new BoundingSphere(Origin, ExtentSphere.Radius);
            }
        }

        public Boolean PointInBox(Vector3 point)
        {
            Vector3 boxSpacePoint = (Vector3)Vector3.Transform(point - Origin, InvertedRotationMatrix);
			return System.Math.Abs(boxSpacePoint.X) <= Extents.X && System.Math.Abs(boxSpacePoint.Y) <= Extents.Y && System.Math.Abs(boxSpacePoint.Z) <= Extents.Z;
        }

        public Boolean LineInBox(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 boxSpaceStartPoint = (Vector3)Vector3.Transform(startPoint - Origin, InvertedRotationMatrix);
            Vector3 boxSpaceEndPoint = (Vector3)Vector3.Transform(endPoint - Origin, InvertedRotationMatrix);

            Vector3 lMid = (boxSpaceStartPoint + boxSpaceEndPoint) * 0.5f;
            Vector3 l = (boxSpaceStartPoint - lMid);
			Vector3 lineExtent = new Vector3(System.Math.Abs(l.X), System.Math.Abs(l.Y), System.Math.Abs(l.Z));

            if (System.Math.Abs(lMid.X) > Extents.X + lineExtent.X) return false;
			if (System.Math.Abs(lMid.Y) > Extents.Y + lineExtent.Y) return false;
			if (System.Math.Abs(lMid.Z) > Extents.Z + lineExtent.Z) return false;

			if (System.Math.Abs(lMid.Y * l.Z - lMid.Z * l.Y) > (Extents.Y * lineExtent.Z + Extents.Z * lineExtent.Y)) return false;
			if (System.Math.Abs(lMid.X * l.Z - lMid.Z * l.X) > (Extents.X * lineExtent.Z + Extents.Z * lineExtent.X)) return false;
			if (System.Math.Abs(lMid.X * l.Y - lMid.Y * l.X) > (Extents.X * lineExtent.Y + Extents.Y * lineExtent.X)) return false;

            return true;
        }

        public Boolean IsBoxVisibleToPoint(Vector3 startPoint, OrientedBoundingBox blockingBox)
        {
            Boolean isBlocked = Corners.All(t => blockingBox.LineInBox(startPoint, t));

            if (!blockingBox.LineInBox(startPoint, blockingBox.Origin)) isBlocked = false;

            return isBlocked;
        }

        public Vector3 LineImpactVector(Vector3 startPoint, Vector3 endPoint)
        {
            Vector3 impactPoint = endPoint;
            Vector3 direction = Vector3.Normalize(endPoint - startPoint);

            while (PointInBox(impactPoint))
            {
                impactPoint -= direction;
            }

            return impactPoint;
        }

        public Single DistanceFromPointToClosestCorner(Vector3 point)
        {
            Single distance = 2147483647;
            Single pointDistance;

            for (Byte i = 0; i < 8; i++)
            {
                pointDistance = Vector3.Distance(point, Corners[i]);
                if (pointDistance < distance) distance = pointDistance;
            }

            pointDistance = Vector3.Distance(point, Origin);
            if (pointDistance < distance) distance = pointDistance;

            return distance;
        }

        public Boolean Collides(OrientedBoundingBox box)
        {
            BoundingSphere boxSphere = box.ExtentSphere;

            switch (ExtentSphere.Contains(ref boxSphere))
            {
                case ContainmentType.Disjoint:
                {
                    return false;
                }
            }

            for (Int32 i = 0; i < 4; i++)
            {
                if (LineInBox(box.Corners[i], box.Corners[i + 4]) || box.LineInBox(Corners[i], Corners[i + 4]))
                {
                    return true;
                }
            }

            for (Int32 i = 0; i < 8; i++)
            {
                Int32 pIndex;

                switch (i)
                {
                    case 3:
                        {
                            pIndex = 0;
                            break;
                        }
                    case 7:
                        {
                            pIndex = 4;
                            break;
                        }
                    default:
                        {
                            pIndex = i + 1;
                            break;
                        }
                }

                if (LineInBox(box.Corners[i], box.Corners[pIndex]) || box.LineInBox(Corners[i], Corners[pIndex]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
