using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prtcl_Generator : MonoBehaviour {

        // VARIABLS
    
    protected int genStartCount = 0;
    protected int genEndCount = 0;

    protected bool started;

    // Audio    
    protected AudioSource source;                                 // The audio source

        // SETINGS

    public bool loop;                       // Generator repeats when everything is instantiate
    public float loopTime = 0;              // Time before repeating

    public bool _______________;

    public List<Prtcl> PrtclList;           // List of prefabs to be generated

    public bool ________________;

    public List<float> minPrtcl;              // Min number of these Prtcls to generate
    public List<float> maxPrtcl;              // Max number of these Prtcls to generate, Default to 1

    public bool _________________;

    public List<float> minTime;             // Min time from generate() that the particle may be instantiated
    public List<float> maxTime;             // Max time from generate() that the particle may be instantiated

    public bool __________________;

    public List<float> minDistance;         // Min distance from from this the Prtcl may be instantiated
    public List<float> maxDistance;         // Max distance from from this the Prtcl may be instantiated

    public bool ___________________;

    public bool makeChild;    
    

	void Awake() {

        if (PrtclList == null) PrtclList = new List<Prtcl>();

        if (minPrtcl == null) minPrtcl = new List<float>(PrtclList.Count);
        if (maxPrtcl == null) maxPrtcl = new List<float>(PrtclList.Count);

        if (minTime == null) minTime = new List<float>(PrtclList.Count);
        if (maxTime == null) maxTime = new List<float>(PrtclList.Count);

        // Fill empty spaces in minPrtcl/maxPrtcl list
        if (PrtclList.Count > minPrtcl.Count) {
            for (int i = minPrtcl.Count; i < PrtclList.Count; i++) {
                minPrtcl.Add(1);
            }
        }
        if (PrtclList.Count > maxPrtcl.Count) {
            for (int i = maxPrtcl.Count; i < PrtclList.Count; i++) {
                maxPrtcl.Add(1);
            }
        }        

        // Fill empty spaces in minTime/maxTime list
        if (PrtclList.Count > minTime.Count) {
            for (int i = minTime.Count; i < PrtclList.Count; i++) {
                minTime.Add(0.0F);
            }
        }
        if (PrtclList.Count > maxTime.Count) {
            for (int i = maxTime.Count; i < PrtclList.Count; i++) {
                maxTime.Add(0.0F);
            }
        }

        // Fill empty spaces in minDistance/maxDistance list
        if (PrtclList.Count > minDistance.Count) {
            for (int i = minDistance.Count; i < PrtclList.Count; i++) {
                minDistance.Add(0.0F);
            }
        }
        if (PrtclList.Count > maxDistance.Count) {
            for (int i = maxDistance.Count; i < PrtclList.Count; i++) {
                maxDistance.Add(0.0F);
            }
        }        
        
	}

    void Start() {

        started = true;

        genStartCount = 0;
        genEndCount = 0;               

        // Generate all listed Prtcls
        for (int i = 0; i < PrtclList.Count; i++) {
            
            // Generate a random number between min and max
            float instNum = Random.Range(minPrtcl[i], maxPrtcl[i]);            
            for (int j = 0; j < instNum; j++) {
                StartCoroutine(generate(i));
                genStartCount++;
            }
        }

        if (!loop)
            Destroy(gameObject, 5f);

        source = GetComponent<AudioSource>();

    }

    void Update() {

        // If all generation IEnumerators have finished, destroy this
        if (genStartCount == genEndCount && !loop && !source.isPlaying && !GetComponent<Prtcl>())
            Destroy(gameObject);
        else if (genStartCount == genEndCount && loop && started) {
            started = false;
            StartCoroutine(waitThenLoop());
        }
      
	}

    public IEnumerator generate(int i) {
        
        yield return new WaitForSeconds(Random.Range(minTime[i], maxTime[i]));

        int num1 = 0; if (Random.value < 0.5f) num1 = 1; else num1 = -1;       // Negative or Positive
        int num2 = 0; if (Random.value < 0.5f) num2 = 1; else num2 = -1;       // Negative or Positive        

        if (PrtclList[i] == null) {
            yield break;
        }        

        Prtcl prtcl;
        prtcl = Instantiate(PrtclList[i], new Vector3(transform.position.x + Random.Range(minDistance[i], maxDistance[i]) * num1, transform.position.y + Random.Range(minDistance[i], maxDistance[i]) * num2, transform.position.z), transform.rotation) as Prtcl;

        if (makeChild) {
            prtcl.gameObject.transform.parent = transform;
        }
        
        genEndCount++;
    }

    IEnumerator waitThenLoop() {

        yield return new WaitForSeconds(loopTime);

        Start();

    }    
}