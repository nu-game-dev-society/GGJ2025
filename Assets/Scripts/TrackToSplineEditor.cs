using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[CustomEditor(typeof(TrackToSpline))]
public class TrackToSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Spline"))
        {
            TrackToSpline trackToSpline = (TrackToSpline)target;
            SplineContainer splineContainer = trackToSpline.GetComponent<SplineContainer>();

            // Remove old splines
            foreach (Spline tmpSpline in splineContainer.Splines)
            {
                splineContainer.RemoveSpline(tmpSpline);
            }

            Spline spline = new Spline(trackToSpline.tracks.Count, true);

            foreach (GameObject track in trackToSpline.tracks)
            {
                Renderer renderer = track.transform.GetComponent<Renderer>();

                if (!track.name.StartsWith("Corner90"))
                {
                    spline.Add(renderer.bounds.center);
                    continue;
                }

                // Handle Corner90
                Vector3 finalPos = renderer.bounds.center;

                Vector3 min = renderer.bounds.min;
                Vector3 max = renderer.bounds.max;

                Vector3 diff = (max - min);
                diff.Scale(track.transform.TransformDirection(new Vector3(-0.2f, 0, 0.2f)));

                finalPos += diff;

                spline.Add(finalPos);

            }
            
            splineContainer.AddSpline(spline);
        }
    }
}
