using System.Collections.Generic;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Union;
using NetTopologySuite.Tests.NUnit.Utilities;
using NUnit.Framework;

namespace NetTopologySuite.Tests.NUnit.Operation.Union
{
    /// <summary>
    /// Large-scale tests of <see cref="CascadedPolygonUnion"/>
    /// using synthetic datasets.
    /// </summary>
    /// <author>mbdavis</author>
    public class CascadedPolygonUnionTest
    {
        readonly GeometryFactory _geomFact = new GeometryFactory();

        [Test]
        public void TestBoxes()
        {
            RunTest(IOUtil.ReadWKT(
                    new[] {
                        "POLYGON ((80 260, 200 260, 200 30, 80 30, 80 260))",
                        "POLYGON ((30 180, 300 180, 300 110, 30 110, 30 180))",
                        "POLYGON ((30 280, 30 150, 140 150, 140 280, 30 280))"
                    }),
                    CascadedPolygonUnionTester.MinSimilarityMeaure);
        }

        [Test]
        public void TestDiscs1()
        {
            var geoms = CreateDiscs(5, 0.7);

            // TestContext.WriteLine(_geomFact.BuildGeometry(geoms));

            RunTest(geoms,
                    CascadedPolygonUnionTester.MinSimilarityMeaure);
        }

        [Test]
        public void TestDiscs2()
        {
            var geoms = CreateDiscs(5, 0.55);

            // TestContext.WriteLine(_geomFact.BuildGeometry(geoms));

            RunTest(geoms,
                    CascadedPolygonUnionTester.MinSimilarityMeaure);
        }

        // TODO: add some synthetic tests

        private static CascadedPolygonUnionTester tester = new CascadedPolygonUnionTester();

        private void RunTest(IList<Geometry> geoms, double minimumMeasure)
        {
            Assert.IsTrue(tester.Test(geoms, minimumMeasure));
        }

        private IList<Geometry> CreateDiscs(int num, double radius)
        {
            var geoms = new List<Geometry>();
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    var pt = new Coordinate(i, j);
                    Geometry ptGeom = _geomFact.CreatePoint(pt);
                    var disc = ptGeom.Buffer(radius);
                    geoms.Add(disc);
                }
            }
            return geoms;
        }
    }
}
