/// <summary>
/// Player skill.
/// This script use for create a hero skill
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkill : MonoBehaviour {
	
	public enum AddAttribute {hp,mp,atk,def,spd,hit,criticalRate,atkSpd,atkRange,moveSpd}
	public enum SkillType {LockTarget,FreeTarget,Instance};
	
	public enum SkillTypeSupport {Buff,Heal};
	
	public enum TargetSkill {SingleTarget,MultipleTarget};
	
	[System.Serializable]
	public class PassiveSkill
	{
		public string skillName = "Passive Skill";
		public int skillID;
		public int unlockLevel = 1;
		public Texture2D skillIcon;
		public AddAttribute addAttribute;
		public float addValue;
		[Multiline]
		public string description = "This is passive skill.";
		public string typeSkill = "Passive";
		
		[HideInInspector]
		public bool isAdd;
	}
	
	[System.Serializable]
	public class ActiveSkillAttack
	{
		public string skillName = "Skill Attack";
		public int skillID;
		public int unlockLevel = 1;
		public Texture2D skillIcon;
		public int mpUse;
		public TargetSkill targetSkill = PlayerSkill.TargetSkill.SingleTarget;
		public float skillArea;
		public SkillType skillType = PlayerSkill.SkillType.LockTarget;
		public float skillRange;
		public AnimationClip animationAttack;
		public float speedAnimation = 1;
		public float castTime = 0;
		public float attackTimer = 0.5f;
		public float multipleDamage = 1;
		public float flichValue = 0;
		public int skillAccurate;
		[Multiline]
		public string description = "This is active skill.";
		public string typeSkill = "Active";
		
		public bool speedTuning;
		
		public GameObject skillFX;
		public AudioClip soundFX;
		
		public GameObject skillFxTarget;
		public AudioClip soundFxTarget;
		
		[HideInInspector]
		public bool isAdd;
		[HideInInspector]
		public float castTimer;
	}
	
	[System.Serializable]
	public class ActiveSkillSupport
	{
		public string skillName = "Skill Support";
		public int skillID;
		public int unlockLevel = 1;
		public Texture2D skillIcon;
		public int mpUse;
		public SkillTypeSupport skillTypeSupport = PlayerSkill.SkillTypeSupport.Buff;
		public AnimationClip animationBuff;
		public float speedAnimation = 1;
		public float castTime = 0;
		public float activeTimer = 0.5f;
		public float duration;
		public AddAttribute addAttribute;
		public float addValue;
		[Multiline]
		public string description = "This is Support skill.";
		public string typeSkill = "Support";
		
		public GameObject buffFX;
		public AudioClip soundFX;
		
		[HideInInspector]
		public bool isAdd;
		[HideInInspector]
		public float castTimer;
		
	}
	
	[System.Serializable]
	public class DurationBuff
	{
		public string buffName;
		public int skillIndex;
		public Texture2D skillIcon;
		public float duration;
		public bool isCount;
		public float durationTimer;	
		
	}

	public List<PassiveSkill> passiveSkill = new List<PassiveSkill>();  //Passive skill
	public List<ActiveSkillAttack> activeSkillAttack = new List<ActiveSkillAttack>(); //Active Attack Skill
	public List<ActiveSkillSupport> activeSkillSupport = new List<ActiveSkillSupport>(); //Buff Skill
	
	[HideInInspector]
	public DurationBuff[] durationBuff  = new DurationBuff[20];
	
	
	//Private Variable
	private HeroController controller;
	private PlayerStatus playerStatus;
	private BottomBar botBar;
	private AnimationManager animationManager;
	private bool setupSkillTimer;
	[HideInInspector]
	public bool oneShotResetTarget;
	
	
	[HideInInspector]
	public bool canCast;
	
	
	private string typeofSkill;
	private int currentUseSkill;
	private GameObject magicCircle;
	
	
	//Editor Variable
	[HideInInspector]
	public int sizePassiveSkill=0;
	[HideInInspector]
	public int sizeActiveAttack=0;
	[HideInInspector]
	public int sizeActiveSupport=0;
	[HideInInspector]
	public List<bool> showPassiveSize = new List<bool>();
	[HideInInspector]
	public List<bool> showActiveAttackSize = new List<bool>();
	[HideInInspector]
	public List<bool> showActiveSupportSize = new List<bool>();
	
	
	// Use this for initialization
	void Start () {

		GameObject go;
		go = GameObject.Find("GUI Manager/HeroBotBar");
		botBar = go.GetComponent<BottomBar>();
		
		controller = this.GetComponent<HeroController>();
		playerStatus = this.GetComponent<PlayerStatus>();
		animationManager = this.GetComponent<AnimationManager>();
		
		UpdatePassiveStatus();
	
	}
	
	// Update is called once per frame
	void Update () {
				
		CountDurationBuff();
	
	}
	
	//Update passive status method
	void UpdatePassiveStatus()
	{
		for(int i=0;i<passiveSkill.Count;i++)
		{
			if(passiveSkill[i].unlockLevel >= playerStatus.status.lv && !passiveSkill[i].isAdd)
			{
				CheckAddAttributePassive(i);
				passiveSkill[i].isAdd = true;
			}
			
			i++;
			
		}
	}
	
	void CheckAddAttributePassive(int indexSkill)
	{
		//Add HP
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.hp)
		{	
			playerStatus.statusAdd.hp += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add MP
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.mp)
		{
			playerStatus.statusAdd.mp += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add Attack
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.atk)
		{
			playerStatus.statusAdd.atk += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add Defend
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.def)
		{
			playerStatus.statusAdd.def += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add Speed
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.spd)
		{
			playerStatus.statusAdd.spd += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add Hit
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.hit)
		{
			playerStatus.statusAdd.hit += Mathf.FloorToInt(passiveSkill[indexSkill].addValue);	
		}
		
		//Add CriticalRate
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.criticalRate)
		{
			playerStatus.statusAdd.criticalRate += passiveSkill[indexSkill].addValue;	
		}
		
		//Add Attack Speed
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.atkSpd)
		{
			playerStatus.statusAdd.atkSpd += passiveSkill[indexSkill].addValue;	
		}
		
		//Add Attack Range
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.atkRange)
		{
			playerStatus.statusAdd.atkRange += passiveSkill[indexSkill].addValue;	
		}
		
		//Add Move Speed
		if(passiveSkill[indexSkill].addAttribute == AddAttribute.moveSpd)
		{
			playerStatus.statusAdd.movespd += passiveSkill[indexSkill].addValue;	
		}
		
	}
	
	void CheckAddAttributeBuff(int indexSkill,string command)
	{
		//Add HP
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.hp)
		{	
			if(command == "Buff")
			playerStatus.statusAdd.hp += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
			if(command == "Debuff")
			playerStatus.statusAdd.hp -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
		}
		
		//Add MP
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.mp)
		{
			if(command == "Buff")
			playerStatus.statusAdd.mp += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);
			if(command == "Debuff")
			playerStatus.statusAdd.mp -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
		}
		
		//Add Attack
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.atk)
		{
			if(command == "Buff")
			playerStatus.statusAdd.atk += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
			if(command == "Debuff")
			playerStatus.statusAdd.atk -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
		}
		
		//Add Defend
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.def)
		{
			if(command == "Buff")
			playerStatus.statusAdd.def += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);
			if(command == "Debuff")
			playerStatus.statusAdd.def -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);
		}
		
		//Add Speed
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.spd)
		{
			if(command == "Buff")
			playerStatus.statusAdd.spd += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
			if(command == "Debuff")
			playerStatus.statusAdd.spd -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
		}
		
		//Add Hit
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.hit)
		{
			if(command == "Buff")
			playerStatus.statusAdd.hit += Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);	
			if(command == "Debuff")
			playerStatus.statusAdd.hit -= Mathf.FloorToInt(activeSkillSupport[indexSkill].addValue);
		}
		
		//Add CriticalRate
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.criticalRate)
		{
			if(command == "Buff")
			playerStatus.statusAdd.criticalRate += activeSkillSupport[indexSkill].addValue;	
			if(command == "Debuff")
			playerStatus.statusAdd.criticalRate -= activeSkillSupport[indexSkill].addValue;	
		}
		
		//Add Attack Speed
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.atkSpd)
		{
			if(command == "Buff")
			playerStatus.statusAdd.atkSpd += activeSkillSupport[indexSkill].addValue;	
			if(command == "Debuff")
			playerStatus.statusAdd.atkSpd -= activeSkillSupport[indexSkill].addValue;	
		}
		
		//Add Attack Range
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.atkRange)
		{
			if(command == "Buff")
			playerStatus.statusAdd.atkRange += activeSkillSupport[indexSkill].addValue;	
			if(command == "Debuff")
			playerStatus.statusAdd.atkRange -= activeSkillSupport[indexSkill].addValue;	
		}
		
		//Add Move Speed
		if(activeSkillSupport[indexSkill].addAttribute == AddAttribute.moveSpd)
		{
			if(command == "Buff")
			playerStatus.statusAdd.movespd += activeSkillSupport[indexSkill].addValue;	
			if(command == "Debuff")
			playerStatus.statusAdd.movespd -= activeSkillSupport[indexSkill].addValue;	
		}
		
	}
	
	//Find skill type
	public string FindSkillType(int skillID)
	{
		for(int i=0;i<passiveSkill.Count;i++)
		{
			if(skillID == passiveSkill[i].skillID)
			{
				typeofSkill = "Passive";
				return typeofSkill;
			}
		}
		
		//Find current use skill in buff type
		for(int i=0;i<activeSkillSupport.Count;i++)
		{
			if(skillID == activeSkillSupport[i].skillID)
			{
				typeofSkill = "Support";
				return typeofSkill;
			}
			
		}
		
		for(int i=0;i<activeSkillAttack.Count;i++)
		{
			if(skillID == activeSkillAttack[i].skillID)
			{
				typeofSkill = "Attack";
				return typeofSkill;
			}
		}
		
		return "";
	}
	
	//Find skill ID
	public int FindSkillIndex(int skillID)
	{
		for(int i=0;i<passiveSkill.Count;i++)
		{
			if(skillID == passiveSkill[i].skillID)
			{
				currentUseSkill = i;
				return currentUseSkill;
			}

		}
		
		//Find current use skill in buff type
		for(int i=0;i<activeSkillSupport.Count;i++)
		{
			if(skillID == activeSkillSupport[i].skillID)
			{
				currentUseSkill = i;
				return currentUseSkill;
			}

		}
		
		for(int i=0;i<activeSkillAttack.Count;i++)
		{
			if(skillID == activeSkillAttack[i].skillID)
			{
				currentUseSkill = i;
				return currentUseSkill;
			}

		}
		
		return 0;
	}
	
	//Cast Break Method
	public void CastBreak()
	{
		if(magicCircle != null)
			Destroy(magicCircle);
		controller.target = null;
		setupSkillTimer = false;
		canCast = false;
		botBar.showCastBar = false;
	}
	
	//Cast skill method
	public void CastSkill(string skillType,int indexSkill)
	{
		if(!setupSkillTimer)
		{
			if(skillType == "Support")
			{
				if(playerStatus.statusCal.mp < activeSkillSupport[indexSkill].mpUse)
				{
					LogText.Instance.SetLog(GameSetting.Instance.logTimer,GameSetting.Instance.logSettingNoMP);
					canCast = false;
				}else
				{
					controller.ctrlAnimState = HeroController.ControlAnimationState.Cast;
					playerStatus.statusCal.mp -= activeSkillSupport[indexSkill].mpUse;
					canCast = true;	
				}
			}else
			
			if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.Instance) 
			{
				if(playerStatus.statusCal.mp < activeSkillAttack[indexSkill].mpUse)
				{
					LogText.Instance.SetLog(GameSetting.Instance.logTimer,GameSetting.Instance.logSettingNoMP);
					canCast = false;
				}else
				{
					controller.ResetBeforeCast();
					controller.ResetOldCast();
					playerStatus.statusCal.mp -= activeSkillAttack[indexSkill].mpUse;
					canCast = true;	
				}
			}else
			
			if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.LockTarget) 
			{
				if(playerStatus.statusCal.mp < activeSkillAttack[indexSkill].mpUse)
				{
					LogText.Instance.SetLog(GameSetting.Instance.logTimer,GameSetting.Instance.logSettingNoMP);
					canCast = false;
				}else
				{
					if(!oneShotResetTarget)
					{
						controller.ResetBeforeCast();
						controller.ResetOldCast();
						controller.useSkill = true;
						controller.skillRange = activeSkillAttack[indexSkill].skillRange;
						GameSetting.Instance.SetMouseCursor(2);
						controller.castid = activeSkillAttack[indexSkill].skillID;
						oneShotResetTarget = true;	
					}
					
				}
			}
			
			if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.FreeTarget) 
			{
				if(playerStatus.statusCal.mp < activeSkillAttack[indexSkill].mpUse)
				{
					LogText.Instance.SetLog(GameSetting.Instance.logTimer,GameSetting.Instance.logSettingNoMP);
					canCast = false;
				}else
				{
					if(!oneShotResetTarget)
					{
					controller.ResetBeforeCast();
					controller.ResetOldCast();
					controller.useSkill = true;
					controller.useFreeSkill = true;
					controller.skillRange = activeSkillAttack[indexSkill].skillRange;
					GameSetting.Instance.SetMouseCursor(2);
					controller.castid = activeSkillAttack[indexSkill].skillID;
					
					
					GameSetting.Instance.SetMouseCursor(3);
					AreaSkillCursor skillCursor;
					skillCursor = GameSetting.Instance.areaSkillCursorObj.GetComponent<AreaSkillCursor>();
					skillCursor.ConvertSizeSkillArea(activeSkillAttack[indexSkill].skillArea);
					oneShotResetTarget = true;
					}
					
				}
			}
			
			//Show magic circle if cast timer more than 0 seconds
			if(canCast)
			{
				if(skillType == "Support" && activeSkillSupport[indexSkill].castTime > 0)
				{
					if(activeSkillSupport[indexSkill].castTime > 0)
					{
						SoundManager.instance.PlayingSound("Cast_Skill");
						if(magicCircle != null)
							Destroy(magicCircle);
						magicCircle = (GameObject)Instantiate(GameSetting.Instance.castEffect,new Vector3(transform.position.x,transform.position.y+0.01f,transform.position.z),Quaternion.identity);
						setupSkillTimer = true;	
						botBar.showCastBar = true;
						activeSkillSupport[indexSkill].castTimer = 0;
					}
				}else if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.Instance && activeSkillAttack[indexSkill].castTime > 0)
				{
					SoundManager.instance.PlayingSound("Cast_Skill");
					if(magicCircle != null)
							Destroy(magicCircle);
					magicCircle = (GameObject)Instantiate(GameSetting.Instance.castEffect,new Vector3(transform.position.x,transform.position.y+0.01f,transform.position.z),Quaternion.identity);
					setupSkillTimer = true;	
					botBar.showCastBar = true;
					activeSkillAttack[indexSkill].castTimer = 0;
				}else if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.LockTarget && activeSkillAttack[indexSkill].castTime > 0)
				{
					SoundManager.instance.PlayingSound("Cast_Skill");
					playerStatus.statusCal.mp -= activeSkillAttack[indexSkill].mpUse;
					GameSetting.Instance.SetMouseCursor(0);
					if(magicCircle != null)
							Destroy(magicCircle);
					magicCircle = (GameObject)Instantiate(GameSetting.Instance.castEffect,new Vector3(transform.position.x,transform.position.y+0.01f,transform.position.z),Quaternion.identity);
					setupSkillTimer = true;	
					botBar.showCastBar = true;
					activeSkillAttack[indexSkill].castTimer = 0;
				}else if(skillType == "Attack" && activeSkillAttack[indexSkill].skillType == SkillType.FreeTarget && activeSkillAttack[indexSkill].castTime > 0)
				{
					SoundManager.instance.PlayingSound("Cast_Skill");
					playerStatus.statusCal.mp -= activeSkillAttack[indexSkill].mpUse;
					GameSetting.Instance.SetMouseCursor(0);
					if(magicCircle != null)
							Destroy(magicCircle);
					magicCircle = (GameObject)Instantiate(GameSetting.Instance.castEffect,new Vector3(transform.position.x,transform.position.y+0.01f,transform.position.z),Quaternion.identity);
					setupSkillTimer = true;	
					botBar.showCastBar = true;
					activeSkillAttack[indexSkill].castTimer = 0;
				}
				

			}
		}
		
		if(canCast)
		{	
			if(skillType == "Support")
			{
				botBar.currentCastTime = activeSkillSupport[indexSkill].castTimer;
				botBar.castTime = activeSkillSupport[indexSkill].castTime;
				
			if(activeSkillSupport[indexSkill].castTimer < activeSkillSupport[indexSkill].castTime)
			{
				activeSkillSupport[indexSkill].castTimer += Time.deltaTime;
			}else if(activeSkillSupport[indexSkill].castTimer >= activeSkillSupport[indexSkill].castTime)
			{
				activeSkillSupport[indexSkill].castTimer = 0;
				SendParameterSkill(skillType,indexSkill);
				controller.ctrlAnimState = HeroController.ControlAnimationState.ActiveSkill;
				setupSkillTimer = false;
				canCast = false;
				botBar.showCastBar = false;
			}
			
			
			}else if(skillType == "Attack")
			{
				botBar.currentCastTime = activeSkillAttack[indexSkill].castTimer;
				botBar.castTime = activeSkillAttack[indexSkill].castTime;
				
				if(activeSkillAttack[indexSkill].castTimer < activeSkillAttack[indexSkill].castTime)
				{
					activeSkillAttack[indexSkill].castTimer += Time.deltaTime;
				}else if(activeSkillAttack[indexSkill].castTimer >= activeSkillAttack[indexSkill].castTime)
				{
					activeSkillAttack[indexSkill].castTimer = 0;
					SendParameterSkill(skillType,indexSkill);
					controller.ctrlAnimState = HeroController.ControlAnimationState.ActiveSkill;
					setupSkillTimer = false;
					canCast = false;
					botBar.showCastBar = false;
				}
			}
		}else
		{
			controller.ctrlAnimState = HeroController.ControlAnimationState.Idle;
		}
		
	}
	
	
	//Send parameter when use skill to animation manager script
	void SendParameterSkill(string skillType,int indexSkill)
	{
		if(skillType == "Support")
		{
			animationManager.skillSetup.skillType = skillType;
			animationManager.skillSetup.skillIndex = indexSkill;
			animationManager.skillSetup.animationSkill = activeSkillSupport[indexSkill].animationBuff;
			animationManager.skillSetup.speedAnimation = activeSkillSupport[indexSkill].speedAnimation;
			animationManager.skillSetup.activeTimer = activeSkillSupport[indexSkill].activeTimer;
			animationManager.skillSetup.speedTuning = false;
			
		}else if(skillType == "Attack")
		{
			animationManager.skillSetup.skillType = skillType;
			animationManager.skillSetup.skillIndex = indexSkill;
			animationManager.skillSetup.animationSkill = activeSkillAttack[indexSkill].animationAttack;
			animationManager.skillSetup.speedAnimation = activeSkillAttack[indexSkill].speedAnimation;
			animationManager.skillSetup.activeTimer = activeSkillAttack[indexSkill].attackTimer;
			animationManager.skillSetup.speedTuning = activeSkillAttack[indexSkill].speedTuning;
		}
	}
	
	//Active Skill method
	public void ActiveSkill(string skillType,int indexSkill)
	{
		if(magicCircle != null)
			Destroy(magicCircle);
		
		if(skillType == "Support")
		{
			
			if(activeSkillSupport[indexSkill].skillTypeSupport == SkillTypeSupport.Buff)
			{
				if(activeSkillSupport[indexSkill].buffFX != null)
				Instantiate(activeSkillSupport[indexSkill].buffFX,transform.position,Quaternion.identity);
				
				if(activeSkillSupport[indexSkill].soundFX != null)
				AudioSource.PlayClipAtPoint(activeSkillSupport[indexSkill].soundFX,transform.position);
				
				
				if(!activeSkillSupport[indexSkill].isAdd)
				{
					CheckAddAttributeBuff(indexSkill,"Buff");
					activeSkillSupport[indexSkill].isAdd = true;
				}
				SentBuffParameter(indexSkill);
				playerStatus.UpdateAttribute();
			}else if(activeSkillSupport[indexSkill].skillTypeSupport == SkillTypeSupport.Heal)
			{
				if(activeSkillSupport[indexSkill].buffFX != null)
				Instantiate(activeSkillSupport[indexSkill].buffFX,transform.position,Quaternion.identity);
				
				if(activeSkillSupport[indexSkill].soundFX != null)
				AudioSource.PlayClipAtPoint(activeSkillSupport[indexSkill].soundFX,transform.position);
				
				
				InitTextDamage(Color.green,activeSkillSupport[indexSkill].addValue.ToString());
				playerStatus.statusCal.hp += (int)activeSkillSupport[indexSkill].addValue;
				playerStatus.UpdateAttribute();
			}
			
			
			
		}else if(skillType == "Attack")
		{
			if(activeSkillAttack[indexSkill].skillType == SkillType.Instance)
			{
				if(activeSkillAttack[indexSkill].skillFX != null)
				Instantiate(activeSkillAttack[indexSkill].skillFX,transform.position,Quaternion.identity);
				
				if(activeSkillAttack[indexSkill].soundFX != null)
				AudioSource.PlayClipAtPoint(activeSkillAttack[indexSkill].soundFX,transform.position);
				
				if(activeSkillAttack[indexSkill].skillAccurate == 0)
				  activeSkillAttack[indexSkill].skillAccurate = playerStatus.status.hit;
				
				GameObject loadSkillArea = (GameObject)Resources.Load("AreaSkill");
				GameObject areaSkill = (GameObject)Instantiate(loadSkillArea,controller.transform.position,Quaternion.identity);
				SkillArea skillArea;
				skillArea = areaSkill.GetComponent<SkillArea>();
				skillArea.radiusSkill = activeSkillAttack[indexSkill].skillArea;
				skillArea.ReciveParameter(playerStatus.statusCal.atk,activeSkillAttack[indexSkill].multipleDamage,activeSkillAttack[indexSkill].skillAccurate,activeSkillAttack[indexSkill].flichValue
				,activeSkillAttack[indexSkill].skillFxTarget,activeSkillAttack[indexSkill].soundFxTarget);
				skillArea.startSkill = true;
				controller.ResetState();
			}else if(activeSkillAttack[indexSkill].skillType == SkillType.LockTarget)
			{
				if(activeSkillAttack[indexSkill].skillFX != null)
				Instantiate(activeSkillAttack[indexSkill].skillFX,transform.position,Quaternion.identity);
				
				if(activeSkillAttack[indexSkill].soundFX != null)
				AudioSource.PlayClipAtPoint(activeSkillAttack[indexSkill].soundFX,transform.position);
				
				if(activeSkillAttack[indexSkill].skillAccurate == 0)
				  activeSkillAttack[indexSkill].skillAccurate = playerStatus.status.hit;
				
				
				if(activeSkillAttack[indexSkill].targetSkill == TargetSkill.MultipleTarget)
				{
					GameObject loadSkillArea = (GameObject)Resources.Load("AreaSkill");
					GameObject areaSkill = (GameObject)Instantiate(loadSkillArea,controller.target.transform.position,Quaternion.identity);
					SkillArea skillArea;
					skillArea = areaSkill.GetComponent<SkillArea>();
					skillArea.radiusSkill = activeSkillAttack[indexSkill].skillArea;
					skillArea.ReciveParameter(playerStatus.statusCal.atk,activeSkillAttack[indexSkill].multipleDamage,activeSkillAttack[indexSkill].skillAccurate,activeSkillAttack[indexSkill].flichValue
					,activeSkillAttack[indexSkill].skillFxTarget,activeSkillAttack[indexSkill].soundFxTarget);
					skillArea.startSkill = true;
					controller.ResetState();
				}else  if(activeSkillAttack[indexSkill].targetSkill == TargetSkill.SingleTarget)
				{
					if(controller.target != null)
					{
						EnemyController enemy; 
						enemy = controller.target.GetComponent<EnemyController>();
						enemy.EnemyLockTarget(controller.gameObject);
						enemy.GetDamage((playerStatus.statusCal.atk) * activeSkillAttack[indexSkill].multipleDamage ,activeSkillAttack[indexSkill].skillAccurate,activeSkillAttack[indexSkill].flichValue
							,activeSkillAttack[indexSkill].skillFxTarget,activeSkillAttack[indexSkill].soundFxTarget);
					}
					
				} 
				controller.ResetState();
				controller.useSkill = false;
				
			} else if(activeSkillAttack[indexSkill].skillType == SkillType.FreeTarget)
			{
				
				if(activeSkillAttack[indexSkill].skillFX != null)
				Instantiate(activeSkillAttack[indexSkill].skillFX,controller.freePosSkill,Quaternion.identity);
				
				if(activeSkillAttack[indexSkill].soundFX != null)
				AudioSource.PlayClipAtPoint(activeSkillAttack[indexSkill].soundFX,transform.position);
				
				if(activeSkillAttack[indexSkill].skillAccurate == 0)
				  activeSkillAttack[indexSkill].skillAccurate = playerStatus.status.hit;
	
				GameObject loadSkillArea = (GameObject)Resources.Load("AreaSkill");
				GameObject areaSkill = (GameObject)Instantiate(loadSkillArea,controller.freePosSkill,Quaternion.identity);
				SkillArea skillArea;
				skillArea = areaSkill.GetComponent<SkillArea>();
				skillArea.radiusSkill = activeSkillAttack[indexSkill].skillArea;
				skillArea.ReciveParameter(playerStatus.statusCal.atk,activeSkillAttack[indexSkill].multipleDamage,activeSkillAttack[indexSkill].skillAccurate,activeSkillAttack[indexSkill].flichValue
				,activeSkillAttack[indexSkill].skillFxTarget,activeSkillAttack[indexSkill].soundFxTarget);
				skillArea.startSkill = true;
				controller.useSkill = false;
				controller.useFreeSkill = false;
				controller.ResetState();
			} 
			
		}
		
		if(oneShotResetTarget)
			oneShotResetTarget = false;
		
	}
	
	//Send buff parameter
	void SentBuffParameter(int indexSkill)
	{
		for(int i=0;i<durationBuff.Length;i++)
		{
			if(durationBuff[i].buffName == "" || durationBuff[i].buffName == activeSkillSupport[indexSkill].skillName)
			{
				durationBuff[i].buffName = activeSkillSupport[indexSkill].skillName;
				durationBuff[i].skillIndex = indexSkill;
				durationBuff[i].skillIcon = activeSkillSupport[indexSkill].skillIcon;
				durationBuff[i].duration = activeSkillSupport[indexSkill].duration;
				durationBuff[i].isCount = true;
				break;
			}
			
		}
	}
	
	//Count duration buff
	void CountDurationBuff()
	{
		for(int i=0;i<durationBuff.Length;i++)
		{
			if(durationBuff[i].isCount)
			{
				if(durationBuff[i].duration > 0)
				{
					durationBuff[i].duration -= Time.deltaTime;
					
				}else if(durationBuff[i].duration <= 0)
				{
					activeSkillSupport[durationBuff[i].skillIndex].isAdd = false;
					CheckAddAttributeBuff(durationBuff[i].skillIndex,"Debuff");
					playerStatus.UpdateAttribute();
					durationBuff[i].buffName = "";
					durationBuff[i].skillIndex = 0;
					durationBuff[i].duration = 0;
					durationBuff[i].skillIcon = null;
					durationBuff[i].isCount = false;
				}
			}
		}
	}
	
	//Text damage
	public void InitTextDamage(Color colorText,string damageGet){
		// Init text damage
		GameObject loadPref = (GameObject)Resources.Load("TextDamage");
		GameObject go = (GameObject)Instantiate(loadPref, transform.position  + (Vector3.up*1.0f), Quaternion.identity);
		go.GetComponentInChildren<TextDamage>().SetDamage(damageGet, colorText);
	}
	
	
	//Gizmoz Variable
	
	public List<float> skillAreaCurrent = new List<float>();

	
	//Draw Gizmoz
	[ExecuteInEditMode]
	void OnDrawGizmosSelected()
	{
		while(skillAreaCurrent.Count != activeSkillAttack.Count)
			{
				if(activeSkillAttack.Count > skillAreaCurrent.Count)
				{
					skillAreaCurrent.Add(0);
				}
				else
				{
					skillAreaCurrent.RemoveAt(skillAreaCurrent.Count-1);
				}
			}
		
		for(int i=0;i<activeSkillAttack.Count;i++)
		{
			if(skillAreaCurrent[i] != activeSkillAttack[i].skillArea)
			{
				Gizmos.color = new Color(0.5f,0f,0f,0.3f);
				Gizmos.DrawSphere(transform.position,activeSkillAttack[i].skillArea);
			}
			skillAreaCurrent[i] = activeSkillAttack[i].skillArea;
		}
		
		
	}
	
}
