
package connectionScripts
{
	function gameConnection::autoAdminCheck(%client)
		{
			parent::autoAdminCheck(%client);
			
			if(isObject(BootQuestMini)) {
				BootQuestMini.addMember(%client);
			} else {
				BootQuest_CreateMinigame();
			}
				
		}
	function gameConnection::spawnPlayer(%client)
		{
			parent::spawnPlayer(%client);
			
			initializeVars(%client);
			messageClient(%client,"","Well Tarnation!");
			
		}
};

activatePackage(connectionScripts);

function initializeVars(%client) {
    %client.player.thirst = 100;
}

function beginTickLoop() {
    for(%i=0;%i<ClientGroup.getCount();%i++){
        %subClient=ClientGroup.getObject(%i);
        addThirst(%subClient);
    }
    schedule(2000,0,beginTickLoop);
}

function addThirst(%client) {
	if(!%client.player)
		return;
	
    %client.player.thirst -= 2;
	
    commandtoClient(%client,'BottomPrint',"THIRST:" SPC %client.player.thirst SPC "%",2);
    if(%client.player.thirst <= 0) {
		echo("Kill");
        %client.player.Kill();
    }
}

beginTickLoop();


function BootQuest_CreateMinigame()
{
	echo("Creating Bootquest Minigame.");
	
	if(isObject(BootQuestMini))
		BootQuestMini.endGame();
	
	new ScriptObject(BootQuestMini)
	{
		class = "MiniGameSO";
		owner = -1;
		numMembers = 0;
		
		title = "BootQuest Minigame";
		colorIdx = "2";
		inviteOnly = false;
		UseAllPlayersBricks = true;
		PlayersUseOwnBricks = false;
		
		Points_BreakBrick = 0;
		Points_PlantBrick = 0;
		Points_KillPlayer = 0;
		Points_KillSelf = 0;
		Points_Die = 0;
		
		respawnTime = "10000";
		vehiclerespawntime = "10000";
		brickRespawnTime = "30000";
		playerDataBlock = "PlayerNoJet";
		
		useSpawnBricks = true;
		fallingdamage = true;
		weapondamage = true;
		SelfDamage = true;
		VehicleDamage = true;
		brickDamage = true;
		
		enableWand = true;
		EnableBuilding = true;
		enablePainting = true;
		
		
		StartEquip0 = "65";
		StartEquip1 = "75";
		StartEquip2 = "383"; 
		StartEquip3 = 0;
		StartEquip4 = 0;
		
	};
	
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
		BootQuestMini.addMember(ClientGroup.getObject(%a));
}

function Player::ReplenishThirst(%player, %amt)
{
	if(isObject(%player))
	{
		%player.thirst += %amt;
		echo(%amt2);
		echo("Added thirst, current thirst: " SPC %player.thirst);
	}
}

registerOutputEvent("Player", "ReplenishThirst", "int 1 100 50", 0);
