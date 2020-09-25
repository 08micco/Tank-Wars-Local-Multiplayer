using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Serializer de ting der skal opfanges af camera, der vælges i inspectoren
    [SerializeField] 
    Transform[] targets;
    //Bestemmer hvor stor padding der skal minimum skal være til kanten
    [SerializeField] 
    float boundingBoxPadding = 2f;
    //Bestemmer den mindste værdi som cameraet kan zoom ind
    [SerializeField]
    float minimumOrthographicSize = 8f;
    //Bestemmer hvor hurtigt cameraet skal zoom ind
    [SerializeField]
    float zoomSpeed = 20f;
    //Definere et camera
    Camera camera;

    //Får værdierne fra cameraet og sætter det til 2D hvis det ikke er fra start
    void Awake () 
    {
        camera = GetComponent<Camera>();
        camera.orthographic = true;
    }

    //Kører hver frame og returner de udregnede værdier for cameraet igennem de 3 funktioner
    void LateUpdate()
    {
        if(GameObject.FindGameObjectsWithTag("Player1").Length != 0 || GameObject.FindGameObjectsWithTag("Player2").Length != 0)
        {
        Rect boundingBox = CalculateTargetsBoundingBox();
        transform.position = CalculateCameraPosition(boundingBox);
        camera.orthographicSize = CalculateOrthographicSize(boundingBox);
        if(camera.orthographicSize > 5)
        {
            camera.orthographicSize = 5;
        }
        }
    }

    //Beregner en "Boundingbox" der indeholder begge tanks.
    //Dette returner en Rect der indeholder de 2 tanks
    Rect CalculateTargetsBoundingBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        foreach (Transform target in targets) {
            Vector3 position = target.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
    }

    //Beregner cameraets position ud fra den tidligere beregnet boundingbox
    //Ved at bruge centeret af boundingboxen
    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, camera.transform.position.z);
    }

    /*Udregner den nye størrelse af cameraet. Viewporten og boundingboxen er begge centreret rectangler
    Kan vi bruge et punkt fra boundingboxen til at bestemme størrelsen på cameraet.
    Boundingboxen laves om til viewport form
    Lerp for at zoome mere smooth
    Clamp for at lave en minimums size*/
    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = camera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = camera.WorldToViewportPoint(topRight);
       
        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / camera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(camera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
    }
}

