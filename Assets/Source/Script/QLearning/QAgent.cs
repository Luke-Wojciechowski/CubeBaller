using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Replay
{
    public List<float> states;
    public float reward;

	public Replay(float topDistance, float bottomDistance, float r)
	{
		states = new List<float>();
		states.Add(topDistance);
		states.Add(bottomDistance);
		reward = r;
	}
}

public sealed class QAgent : MonoBehaviour {
	
	[SerializeField]
	private GameObject topObstace;
	[SerializeField]
	private GameObject bottomObstace;
	QLearnUnit _qLearnUnit;
	public static event Action OnCrush;
	private float reward = 0.0f;							 
	private List<Replay> replayMemory = new List<Replay>();	 
	private float discount = 0.99f;
	private int mCapacity = 10000;
	private float moveForce = 25f;
	private float timer = 0;								 
	private float maxBalanceTime = 0;
	private bool crashed = false;
	[SerializeField]
	private Transform startPos;
	private Rigidbody rb;

	
	void Start () {
		_qLearnUnit = new QLearnUnit(2,2,1,6,0.2f);
		rb = this.GetComponent<Rigidbody>();
	}


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			Restart();
	}

 

	private void OnCollisionEnter(Collision collisionInfo)
	{
		crashed = true;
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		crashed = true;
	}

	void OnCollisionExit(Collision collisionInfo)
	{
		crashed = false;
	}
	
	void FixedUpdate () {
		timer += Time.deltaTime;
		List<float> states = new List<float>();
		List<float> softMaxValue = new List<float>();
			
		states.Add(Vector3.Distance(this.transform.position,topObstace.transform.position));
		states.Add(Vector3.Distance(this.transform.position,bottomObstace.transform.position));
		
		softMaxValue = SoftMax(_qLearnUnit.CalcOutput(states));
		float maxQ = softMaxValue.Max();
		int maxQIndex = softMaxValue.ToList().IndexOf(maxQ);

		if(maxQIndex == 0)
			rb.AddForce(Vector3.up * moveForce * softMaxValue[maxQIndex]);

		if (crashed)
		{
			reward = -1.0f;
		}
		else
			reward = 0.01f;
		
		Replay lastMemory = new Replay(Vector3.Distance(this.transform.position,topObstace.transform.position),
								Vector3.Distance(this.transform.position,bottomObstace.transform.position),
								reward);

		if(replayMemory.Count > mCapacity)
			replayMemory.RemoveAt(0);
		
		replayMemory.Add(lastMemory);

		if(crashed)
		{
			for(int i = replayMemory.Count - 1; i >= 0; i--)
			{
				List<float> toutputsOld = new List<float>();
				List<float> toutputsNew = new List<float>();
				toutputsOld = SoftMax(_qLearnUnit.CalcOutput(replayMemory[i].states));	

				float maxQOld = toutputsOld.Max();
				int action = toutputsOld.ToList().IndexOf(maxQOld);

			    float feedback;
				if(i == replayMemory.Count-1 || replayMemory[i].reward == -1)
					feedback = replayMemory[i].reward;
				else
				{
					toutputsNew = SoftMax(_qLearnUnit.CalcOutput(replayMemory[i+1].states));
					maxQ = toutputsNew.Max();
					feedback = (replayMemory[i].reward +discount * maxQ);
				} 

				toutputsOld[action] = feedback;
				_qLearnUnit.Train(replayMemory[i].states,toutputsOld);
				
			}
		
			if(timer > maxBalanceTime)
			{
			 	maxBalanceTime = timer;
			    Debug.Log(timer);
			} 

			timer = 0;

			crashed = false;
			Restart();
			replayMemory.Clear();
 
		}	
	}

	void Restart()
	{
		this.transform.position = startPos.position;
		rb.velocity = new Vector3(0,0,0);
		OnCrush?.Invoke();
	}

	List<float> SoftMax(List<float> oSums) 
    {
      var max = oSums.Max();

      float scale = 0.0f;
      for (int i = 0; i < oSums.Count; ++i)
        scale += Mathf.Exp( (oSums[i] - max));

      List<float> result = new List<float>();
      for (int i = 0; i < oSums.Count; ++i)
        result.Add(Mathf.Exp( (oSums[i] - max)) / scale);

      return result; 
    }
}
