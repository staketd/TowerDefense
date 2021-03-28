using System;
using System.Collections.Generic;
using UnityEngine;

namespace Support {
    public class Geometry {
        public static float VectorProduct(Vector2 a, Vector2 b) {
            return a.x * b.y - b.x * a.y;
        }
        public static bool DoSectionAndCircleIntersect(Vector2 ptA, Vector2 ptB, Vector2 circleCenter, float radius) {
            Vector2 AO = circleCenter - ptA;
            Vector2 AB = ptB - ptA;
            float lenAH = Vector2.Dot(AO, AB);
            Vector2 H = ptA + AB.normalized * lenAH;
            if (Vector2.SqrMagnitude(ptA - circleCenter) < radius * radius ||
                Vector2.SqrMagnitude(ptB - circleCenter) < radius * radius) {
                return true;
            }

            if (Vector2.SqrMagnitude(H - circleCenter) >= radius * radius) {
                return false;
            }

            return VectorProduct(H - ptA, H - ptB) < 1e-7 && Vector2.Dot(H - ptA, H - ptB) < 0;
        }

        public static bool DoRectangleAndCircleIntersect(Vector2 ptA, Vector2 ptB, Vector2 circleCenter, float radius) {
            List<float> rectangleNodesXs = new List<float> {
                ptA.x, ptA.x, ptB.x, ptB.x
            };
            List<float> rectangleNodesYs = new List<float> {
                ptA.y, ptB.y, ptB.y, ptA.y
            };
            for (int i = 0; i < 4; ++i) {
                Vector2 pt1 = new Vector2(rectangleNodesXs[i], rectangleNodesYs[i]);
                Vector2 pt2 = new Vector2(rectangleNodesXs[(i + 1) % 4], rectangleNodesYs[(i + 1) % 4]);
                if (DoSectionAndCircleIntersect(pt1, pt2, circleCenter, radius)) {
                    return true;
                }
            }

            return false;
        }
    }
}