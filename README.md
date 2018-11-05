# TXminigame
Minigame project in Tencent game planning course

## Objects
- 人物
- Boss
- 纸张（本身会发亮）
- 原书
- 传送点
- 陷阱

## Interaction
- Boss 在人物附近X米半径内，人物屏幕边缘会变红
- Boss 站上传送点等待X秒后，随机选定传送点传送。中断时传送失败
- Boss 攻击半径内有人物，点击屏幕按到人物，人物被扁平化成2D图片消失，回到原书
- Boss 踩到人物技能使用留下的陷阱，有状态变化。
- 人物 走到纸张附近，按下纸张X秒解锁纸张。若中途离开，解锁时间会被存档。
- 人物 在原书附近，按下原书X秒解救队友，中途离开，解锁时间不存档。
- 人物 使用复活技能，在原书附近X米半径内使用技能都可以使人物复活。
- 人物 消耗纸张使用技能，确定攻击目标/移动方向。有引导时间，中途打断则纸张被浪费，技能不释放。若成功释放，人物状态变化/陷阱实例化。
- 人物 使用基础技能，步骤同上。不同点是技能有冷却时间
- 人物 在原书中呆了X秒无人来救，则死亡。该人物纸张清零，不算做团队成就。
- 人物 点击队友，选择自己拥有的纸张赠送
- 技能 基础技能/书籍技能 
    - 蓝色：设置陷阱 boss碰到会冻住。陷阱是人物可以看到，boss看不到的。人物可以放在Boss的必经之路上。只要Boss不踩到，此陷阱就永久有效。（基础技能）
    - 红色：设置陷阱 boss碰到会行走变缓慢一段时间。（基础技能）
    - 绿色：设置陷阱 boss碰到会黑屏 屏幕上加一个黑色蒙版。（基础技能）
    - 白色：站在原书附近范围X米内使用技能可以使得队友复活。 人物走到该范围内，UI小地图上被封印队友的图标会闪动作为提示。（书籍技能）
    - 黑色：自己隐身，可以隐身无限长时间，不操作移动就可以一直隐身。（书籍技能）
    - 金色：自己闪现一段距离。（书籍技能）
- 系统 用户注册、登陆管理。
- 系统 玩家进入游戏，选择游戏角色
- 系统 玩家死亡/胜利的场景切换 给相应的故事内容
- 系统 视野 2.5D

## UI
- 接收人物移动信息
- 各人物获得的书籍、各人物死亡状态显示在左上角
- 小地图中人的显示
    - 人物：知道队友所在位置、原书位置；使用复活技能是如果走到原书有效半径内，原书在小地图UI上的点会跳动
    - Boss：知道所有未解锁纸张位置、原书位置
- 书籍技能按键
    - 搜集到纸张后点亮技能、赠送纸张后技能清除、技能使用后技能清除
    - 选择使用哪个技能
    - 确定技能释放对象/技能方向/释放的陷阱位置
    - 技能进入引导阶段
    - 技能生效
- 基础技能按键
    - 冷却时间等待
    - 其他如上

## Game Architecture 
![image](flowchart.jpg)

### UI ###

+ 读取人物状态，屏幕显示相应的提示
+ 读取人物技能，显示相应的技能


### 游戏逻辑 ###

+ infomation control
	
	+ 公共变量管理（人物基本技能，血量，状态）
	+ 和UI的交流（UI要获取的信息，以及UI送过来的信息，都通过这个类）
	+ 本地模块间也通过这个类交流（当移动模块需要人物状态时）
	+ 维护一个next_skill_to_execute，每次发动技能前需要设定技能所需的信息
	
	例子：
		
		// 获取人物的状态
		// infomation control中代码
		public static int getState(){
			return this.getCompnent<StateControl>().state;
			// State Control 是维护人物状态的脚本
		}
	其他脚本的调用：
		
		this.getCompnent<InfomationControl>().getState();


+ state control
	+ 人物状态控制
	+ 人物状态改变，至少需要提供 `trans2(int targetState)`
	+ 判断人物状态是否可以变化
	+ 维护人物状态维持时间（冰冻三秒后自动改变状态）

+ skill control
	
	+ 负责发送技能
	+ 负责技能的冷却
	+ 由于纸张都是在技能使用前后丢失或得到，因此，同时管理纸张【救人，捡纸张，抓人  可以都视作技能来实现】
	+ 技能的执行顺序：
		+ UI层获取执行的技能，以及技能的对象或方向或位置
		+ UI层传递信息至information层
		+ 判断技能能否执行
		+ 可以执行，通知state control改变状态`trans2(targetState)
		+ skill state开始执行技能 
	+ 发动技能的两个阶段
		+ BEGIN_SKILL
		+ EXECUTE_SKILL
	+ 注意：
		+ 打断技能时，操作是在状态改变的函数中完成的
		+ 从BEGIN_SKILL或者EXECUTE_SKILL改变状时，需要恢复技能冷却时间和引导剩余时间
		+ 如果为一次性技能，需要将纸张和技能删了

	Skill Class：

		public class SKILL{
		
			private float boot_time = 0;		// 引导时间
			private float cd_time = 0;			// 技能冷却时间
			private bool  once_skill = false;	// 是否为一次性技能
			/* 上面为需要修改的参数 */
	
			private float boot_waiting_time = 0;    		// 技能引导剩余时间
			private float cd_waiting_time = 0;				// 剩余的冷却时间
	
			/*技能引导的目标参数*/
			private GameObject skill_owner = null;
			private GameObject target = null;
			private Vector3 direction = null;
			private Vector3 targetPos = null;
	
			public SKILL(GameObject skill_owner, bool once_skill){
				this.skill_owner = skill_owner;
				this.once_skill = once_skill;
			}
	
	
	
			//几乎不用重载
			public void display(){
				// 技能引导
				if(getState() == BEGIN_SKILL){
					// 正在引导
					if(boot_waiting_time > 0){
						update(boot_waiting_time);   // 计算剩余引导时间
						return;
					}
					boot_waiting_time = boot_time;			// 恢复引导时间
					
					/*更新人物状态*/
					this.getComponent<StateControl>().tran2(EXECUTE_SKILL);
				}
	
				/*获取技能参数*/
				target = this.getComponent<InformationControl>().getTarget;
				targetPos = this.getComponent<InformationControl>().getTargetPos;
				direction = this.getComponent<InformationControl>().getDirection;	
				attack();
			}
			
			
			// 重点重载函数 * 2
	
			// 技能效果函数
			public void  attack(){
				/*结束技能，更新人物状态*/
				this.getComponent<StateControl>().tran2(FREE);
				cd_waiting_time = cd_time;				// 恢复冷却时间
				return;
			}
	
			// 能否使用技能
			public bool CanUse(){
				// 未冷却
				if(cd_waiting_time > 0) 
					return false;
				// 判断target/targetPos/Direction 是否合理
			}
		}


	伪代码：
		
		void Update(){
			UpdateSkillCD();	//刷新技能的冷却时间
			
			if(getState != BEGIN_SKILL || getState != EXECUTE_SKILL){
				return;
			}
			
			// 开始执行技能
			this.getComponent<InformationControl>().getNextSkill().display();	
		}
+ move control
	+ 控制移动
	
	伪代码：
		
		void Update(){
			if(getState() != FREE)
				return;
			Move();
		}

+ animotion control

	+ 根据状态和必要信息，控制动画的播放
	
	伪代码
	
		int lastState = FREE;
		void Update(){
			if(lastState == getState())
				return;
			
			if(getState == BEGIN_SKILL){
				//根据技能播放动画
			}	
			// ...
			lastState = getState();
		}