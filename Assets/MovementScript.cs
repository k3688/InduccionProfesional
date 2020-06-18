﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour{
  private Vector3 inicialPos; //To record subject position
  public float speed = 0.1f;  //To determine subject speed
  public float defaultlifeSpan = 15f;
  public float lifeSpan;
  [Header("Inputs")]
  public float f;     //Forward Input Label
  public float fr;    //Forward-Right Input Label
  public float fl;    //Forward-Left Input Label
  public float l;     //Left Input Label
  public float r;     //Right Input Label
  public float b;     //Back Input Label
  public float br;    //Back-Right Input Label
  public float bl;    //Back-Left Input Label

  [Header("Weights")]

//New stuff
  private float[][,] weightsAStorage = new float [50][,];//[4,8];  //Storage for A weights
  private float[][,] weightsBStorage = new float [50][,];  //Storage for B weights
  public float[,] weightsA = new float [32,8];

  private float[,] BestGeneCodeA = new float [32,8]; //Storage for offspring of generation's best (A weights)
  private float[,] BestGeneCodeB = new float [2,32]; //Storage for offspring of generation's best (B weights)
  public float[,] weightsB = new float [2,32];
//New Stuff  

  //old stuff
  
  //public float[,] weightsA1 = new float [4,8];
  //public float[,] weightsA2 = new float [4,8];
  //public float[,] weightsA3 = new float [4,8];
  //public float[,] weightsABest = new float [4,8];

  
  //public float[,] weightsB1 = new float [4,4];
  //public float[,] weightsB2 = new float [4,4];
  //public float[,] weightsB3 = new float [4,4];
  //public float[,] weightsBBest = new float [4,4];
  //old stuff

 /* public float a00;   //Used to show current thought process{
  public float a01;
  public float a02;
  public float a03;
  public float a04;
  public float a05;
  public float a06;
  public float a07;
  public float a10;
  public float a11;
  public float a12;
  public float a13;
  public float a14;
  public float a15;
  public float a16;
  public float a17;
  public float a20;
  public float a21;
  public float a22;
  public float a23;
  public float a24;
  public float a25;
  public float a26;
  public float a27;
  public float a30;
  public float a31;
  public float a32;
  public float a33;
  public float a34;
  public float a35;
  public float a36;
  public float a37;

  public float b00;
  public float b01;
  public float b02;
  public float b03;
  public float b10;
  public float b11;
  public float b12;
  public float b13; //}*/

  [Header("Genetic")]
  public int individual = 0;
  public int generation = 0;
  public bool mutated;
//New stuff
  private float[] fitnessScoresStorage = new float[50];
//New Stuff  

  public float fitnessCurrent = 0;
 // public float fitnes1; Old stuff
 // public float fitnes2;
 // public float fitnes3;
  public float fitnessParentBest;
  public float fitnessParentB;
  public int parentBest;
  public int parentBestGen;
  public int parentB;
  private float[,] parentBestWA;
  private float[,] parentBestWB;
  private float[,] parentBWA;
  private float[,] parentBWB;

  [Header("Choice")]  //wFix this
  public float hori;
  public float verti;


  [Header("Sensors")]
  public float sen = 100f;

  void Start(){
    inicialPos = transform.position;    //Record current subject position
    weightsA = GenerateWeights(true);
    weightsB = GenerateWeights(false);
     for (int x = 0; x < 50; x++){
     weightsAStorage[x] = new float[32,8];
    }
    for (int x = 0; x < 50; x++){
     weightsBStorage[x] = new float[2,32];
    }
    parentBestWA = GenerateWeights(true);
    parentBestWB = GenerateWeights(false);
    lifeSpan = defaultlifeSpan;
  }

  void Update(){
    //weightVals();
    float[,] inputs = Sensors();
    float[,] output = Decision(inputs);
    //fitnessCurrent -= 0.5f;
    //transform.Translate(0.1f*Input.GetAxis("Horizontal"),0f,0.1f*Input.GetAxis("Vertical"));   //Movement Override

    hori = output[0,0];
    verti = output[1,0];
  
    lifeSpan -= Time.deltaTime;
     if ( lifeSpan < 0 )
     {
         lifeSpan = defaultlifeSpan;
           reset();
     }

    if(output[0,0] >= 0.5f){
      transform.Translate(-speed,0f,0f);
    }else if(output[0,0] < 0.5f){
      transform.Translate(speed,0f,0f);
    }
      
    if(output[1,0] >= 0.5f){
      transform.Translate(0f,0f,-speed);
    }else if (output[1,0] < 0.5f){
      transform.Translate(0f,0f,speed); 
    }    
    
  }


//Supplementary Methods
  public float[,] GenerateWeights(bool isA){
    float[,] wa;
    if(isA){
      wa = new float[32,8];
    }else{
      wa = new float [2,32];
    } 
    for(int i = 0; i< wa.GetLength(0); i++){
      for(int j = 0; j<wa.GetLength(1); j++){
        wa[i,j] = Random.Range(-1f,1f);
      } 
    }
    return wa;
  }

 /* public void weightVals(){
    a00 = weightsA[0,0];
    a01 = weightsA[0,1];
    a02 = weightsA[0,2];
    a03 = weightsA[0,3];
    a04 = weightsA[0,4];
    a05 = weightsA[0,5];
    a06 = weightsA[0,6];
    a07 = weightsA[0,7];
    a10 = weightsA[1,0];
    a11 = weightsA[1,1];
    a12 = weightsA[1,2];
    a13 = weightsA[1,3];
    a14 = weightsA[1,4];
    a15 = weightsA[1,5];
    a16 = weightsA[1,6];
    a17 = weightsA[1,7];
    a20 = weightsA[2,0];
    a21 = weightsA[2,1];
    a22 = weightsA[2,2];
    a23 = weightsA[2,3];
    a24 = weightsA[2,4];
    a25 = weightsA[2,5];
    a26 = weightsA[2,6];
    a27 = weightsA[2,7];
    a30 = weightsA[3,0];
    a31 = weightsA[3,1];
    a32 = weightsA[3,2];
    a33 = weightsA[3,3];
    a34 = weightsA[3,4];
    a35 = weightsA[3,5];
    a36 = weightsA[3,6];
    a37 = weightsA[3,7];

    b00 = weightsB[0,0];
    b01 = weightsB[0,1];
    b02 = weightsB[0,2];
    b03 = weightsB[0,3];
    b10 = weightsB[1,0];
    b11 = weightsB[1,1];
    b12 = weightsB[1,2];
    b13 = weightsB[1,3];
  }*/

  private float[,] Sensors(){
    RaycastHit hit;
    Vector3 sensorStartPos = transform.position;
    float[,] input = new float[8,1];
    if(Physics.Raycast(transform.position, Vector3.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        l = hit.distance;
        input[0,0] = l;
        CalculateFitness(l);
      }else{
        //CalculateFitness(-l);
        input[0,0] = 0f;
        l = 0f;
      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,45,0) * transform.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        fl = hit.distance;
        input[1,0] = fl;
        CalculateFitness(fl);
      }else{
        //CalculateFitness(-fl);
        input[1,0] = 0f;
        fl = 0f;
      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,90,0) * transform.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        f = hit.distance;
        input[2,0] = f;
        CalculateFitness(f);
      }else{
        //CalculateFitness(-f);
        input[2,0] = 0f;
        f = 0f;
      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,135,0) * transform.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        fr = hit.distance;
        input[3,0] = fr;
        CalculateFitness(fr);
      }else{
        //CalculateFitness(-fr);
        input[3,0] = 0f;
        fr = 0f;
      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,180,0) * transform.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        r = hit.distance;
        input[4,0] = r;
        CalculateFitness(r);
      }else{
        //CalculateFitness(-r);
        input[4,0] = 0f;
        r = 0f;

      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,225,0) * transform.forward, out hit, sen)){
      if(hit.collider.gameObject.name == "Ball"){
        br = hit.distance;
        input[5,0] = br;
        CalculateFitness(br);
      }else{
        //CalculateFitness(-br);
        input[5,0] = 0f;
        br = 0f;
      }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,270,0) * transform.forward, out hit, sen)){ 
        if(hit.collider.gameObject.name == "Ball"){
          b = hit.distance;
          input[6,0] = b;
          CalculateFitness(b);
        }else{
          //CalculateFitness(-b);
          input[6,0] = 0f;
          b = 0f;
        }
    }
    Debug.DrawLine(sensorStartPos, hit.point);

    if(Physics.Raycast(transform.position, Quaternion.Euler(0,315,0) * transform.forward, out hit, sen)){
        if(hit.collider.gameObject.name == "Ball"){
          bl = hit.distance;
          input[7,0] = bl;
          CalculateFitness(bl);
        }else{
          //CalculateFitness(-bl);
          input[7,0] = 0f;
          bl = 0f;
        }
    }
    Debug.DrawLine(sensorStartPos, hit.point);
    return input;
  }

  public void CalculateFitness(float dist){
    if(dist > -2f && dist < 0f){
      //fitnessCurrent += dist;
    }/*else if(dist > 0f && dist <= 0.25f){
      fitnessCurrent += 3f; 
    }else if(dist > 0.25f && dist <= 1f){
      fitnessCurrent += 2f; 
    }else if(dist > 1f && dist <= 2f){
      fitnessCurrent += 1f;
    }else if(dist > 2f){
      fitnessCurrent += 0.1f;
    }*/
  }

  public float[,] Decision(float[,] input){
    float[,] weightedInputs = MultiplyMatrix(weightsA, input);
      //should put a bias step here
    float[,] sigmoidHiddenA = Sigmoid(weightedInputs);
    float[,] sigHidAWeighted = MultiplyMatrix(weightsB,sigmoidHiddenA);
      //should put a bias step here
    float[,] results = Sigmoid(sigHidAWeighted);
    return results;
  }

  public float[,] MultiplyMatrix(float[,] a, float[,] b){
    float[,] c=new float[a.GetLength(0),b.GetLength(1)];
    for(int i=0;i<c.GetLength(0);i++){
      for(int j=0;j<c.GetLength(1);j++){
        c[i,j]=0;
        for(int k=0;k<a.GetLength(1);k++) // OR k<b.GetLength(0)
          c[i,j]=c[i,j]+a[i,k]*b[k,j];
      }
    }
      return c;
  }

  public float[,] Sigmoid(float[,] a){
    float[,] ans = a;
    for (int i = 0; i < a.GetLength(0); i++){
      ans[i,0] = 1.0f / (1.0f + (float) Mathf.Exp(-a[i,0]));
    }
    return ans;
  }

  public void OnCollisionEnter(Collision col){
    if (col.gameObject.name == "Em" || col.gameObject.name == "Lou"){
      print("Sorry Em!");
      transform.position = inicialPos;
    }else if (col.gameObject.name == "Ball"){
      transform.position = inicialPos;
      fitnessCurrent += 100;
      lifeSpan = defaultlifeSpan;
     // LearningStep();
    }else if(col.gameObject.name == "RWall" || col.gameObject.name == "LWall" || col.gameObject.name == "BWall" || col.gameObject.name == "FWall"){
      transform.position = inicialPos;
      //fitnessCurrent -= 50;
      lifeSpan = defaultlifeSpan;
      LearningStep();
    }
  }

  public void LearningStep(){
    weightsAStorage[individual] = weightsA;
    weightsBStorage[individual] = weightsB;
    fitnessScoresStorage[individual] = fitnessCurrent;
    fitnessCurrent = 0;

    if(generation == 0){
      weightsA = GenerateWeights(true);
      weightsB = GenerateWeights(false);
    }else{
        Mating(parentBestWA,parentBWA,true);
        Mating(parentBestWB,parentBWB,false);

      if(Random.Range(0f,1f) > 0.75f){
        mutated = true;
        MutationStep(weightsA, false);   
        MutationStep(weightsB, true);//Pending
      }else{
        mutated = false;
      }
    }
    if(individual == 49){
      GeneticStep();
      individual = 0;
      generation += 1;
    }else{
      individual += 1;
    }

/*    switch (individual){
      case 1:
      weightsA1 = weightsA;
      weightsB1 = weightsB;
      fitnes1 = fitness;
      fitness = 0;
      if(generation != 0){
        Mutation(weightsA, false);
        Mutation(weightsB, true);
      }else{
        fitnessBest = fitnes1;
        weightsA = generateWeightsA();
        weightsB = generateWeightsB();
      }
      individual += 1;
      break;
      case 2:
      weightsA2 = weightsA;
      weightsB2 = weightsB;
      fitnes2 = fitness;
      fitness = 0;
     // if(generation != 0){
     //   Mutation(weightsA1, false);
     //   Mutation(weightsB1, true);
     // }else{
        weightsA = generateWeightsA();
        weightsB = generateWeightsB();
     // }
      individual += 1;
      break;
      case 3:
      weightsA3 = weightsA;
      weightsB3 = weightsB;
      fitnes3 = fitness;
      fitness = 0;
      GeneticStep();
      individual = 1; //fix this
      generation += 1;
      break;
    } */ //Old stuff but just in case for code clarification
  }

  public void GeneticStep(){
    float parentfit = fitnessScoresStorage[0];
    int parentIndex = 0;

     for (int i = 1; i < fitnessScoresStorage.Length; i++) {
         if (fitnessScoresStorage[i] > parentfit) {
             parentfit = fitnessScoresStorage[i];
             parentIndex = i;
         }
     }

     if(parentfit > fitnessParentBest){

        fitnessParentB = fitnessParentBest;
        parentB = parentBest;
        parentBWA = parentBestWA;
        parentBWB = parentBestWB; 
        fitnessParentBest = parentfit;
        parentBestWA = weightsAStorage[parentIndex];
        parentBestWB = weightsBStorage[parentIndex];
        parentBest = parentIndex;
        parentBestGen = generation;
     }else{
        fitnessParentB = parentfit;
        parentB = parentIndex;
        parentBWA = weightsAStorage[parentIndex];
        parentBWB = weightsBStorage[parentIndex];
     }

     Mating(parentBestWA,parentBWA,true);
     Mating(parentBestWB,parentBWB,false);
  }

  public void Mating(float[,] a, float[,] b, bool isA){
    for (int k = 0; k < a.GetLength(0); k++){
      for (int l = 0; l < a.GetLength(1); l++){
        if(Random.Range(0f,1f) > 0.50f){
          if(!isA){
            weightsB[k,l] = a[k,l];
          }else{
            weightsA[k,l] = a[k,l];  
          }
        }else{
          if(!isA){
            weightsB[k,l] = b[k,l];
          }else{
            weightsA[k,l] = b[k,l];
          }
        }
      }
    }
  }


  public void MutationStep(float[,] a, bool isB){
    for (int k = 0; k < a.GetLength(0); k++)
    for (int l = 0; l < a.GetLength(1); l++){
      bool chance  = (Random.value > 0.5f);
      if(chance){
        if(isB){
          weightsB[k,l] = Random.Range(-100f,100f);
        }else{
          weightsA[k,l] = Random.Range(-100f,100f);
        }
      }
    }
  }

   public void reset (){
      transform.position = inicialPos;
      fitnessCurrent -= 1000;
      LearningStep();
  }
}
