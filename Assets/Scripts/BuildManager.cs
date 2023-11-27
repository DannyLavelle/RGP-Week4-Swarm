using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    float PlayerHealth = 100;
    public static BuildManager instance;
    private GameObject turretToBuild;
    public GameObject machineTurretPrefab;
    public GameObject sniperTurretPrefab;
    public GameObject missileTurretPrefab;
    public GameObject meleeTurretPrefab;
    public GameObject factoryPrefab;
    public GameObject builderPrefab;
    public Node nodeSelected;
    public GameObject nodeObjectSelected;
    public GameObject buyMenu;
    public GameObject upgradeMenu;
    GameObject MainCamera;
    public TMP_Text upgradeButtonText;
    public TMP_Text HealthText;
    public GameObject[] UiElements;
    //TurretCosts
    float cost;
    float upgradeCost;
    float upgradeIncrease;
  
    public float MachineTurretCost = 100;
    public float MachineTurretUpgradeCost = 100;
    public float MachineTurretBaseUpgradeCost = 100;
    public float MachineTurretUpgradeIncrease = 100;

    public float sniperTurretCost = 200;
    public float sniperTurretUpgradeCost = 200;
    public float sniperTurretBaseUpgradeCost = 200;
    public float sniperTurretUpgradeIncrease = 2;

    public float meleeTurretCost = 300;
    public float meleeTurretUpgradeCost = 300;
    public float meleeTurretBaseUpgradeCost = 300;
    public float meleeTurretUpgradeIncrease = 100;

    public float missileTurretCost = 200;
    public float missileTurretUpgradeCost = 200;
    public float missileTurretBaseUpgradeCost = 200;
    public float missileTurretUpgradeIncrease = 2;

    public float builderCost = 500;
    public float builderUpgradeCost = 250;
    public float builderBaseUpgradeCost = 250;
    public float builderUpgradeIncrease = 2;

    public float factoryCost = 750;
    public float factoryUpgradeCost = 500;
    public float factoryBaseUpgradeCost = 500;
    public float factoryUpgradeIncrease = 2;


    //End of TurretCosts
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;
        UiElements[0].SetActive(false);
        UiElements[1].SetActive(true);
        HealthText.text = ("Health : " + PlayerHealth +"%");
        Currency.amount = 1000;
        MainCamera = Camera.main.gameObject;
        //    turretToBuild = machineTurretPrefab; 
        //
    }
    public GameObject getTurretToBuild()
    {
        return turretToBuild;
    }
    public void setTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }
    public void selectedNode(Node node)
    {
        nodeSelected = node;
    }
    public void BringApproprioteMenu(bool toggle)
    {
        Node node = nodeObjectSelected.GetComponent<Node>();
        if (node.turret == null)
        {
            ToggleBuyMenu(toggle);
        }
        else
        {
            ToggleUpgradeMenu(toggle);
        }
    }
    public void ToggleBuyMenu(bool toggle)
    {
        if (toggle)
        {
            // If the buy menu is currently active, hide it
            HideBuyMenu();
        }
        else
        {
            // If the buy menu is currently inactive, show it
            ShowBuyMenu();
        }
    }
    public void ToggleUpgradeMenu(bool toggle)
    {
        if (toggle)
        {
            // If the upgrade menu is currently active, hide it
            HideUpgradeMenu();
        }
        else
        {
            // If the upgrade menu is currently inactive, show it
            ShowUpgradeMenu();
        }
    }

    void ShowBuyMenu()
    {
        // Set the main camera inactive
        MainCamera.SetActive(false);

        // Set the buy menu active
        buyMenu.SetActive(true);

        // Set the position of the buy menu to match the selected node
        buyMenu.transform.position = new Vector3(nodeObjectSelected.transform.position.x, buyMenu.transform.position.y, nodeObjectSelected.transform.position.z);
    }

    void HideBuyMenu()
    {
        // Set the main camera active
        MainCamera.SetActive(true);

        // Set the buy menu inactive
        buyMenu.SetActive(false);
    }
    void ShowUpgradeMenu()
    {
        MainCamera.SetActive(false);

        // Set the buy menu active
        upgradeMenu.SetActive(true);
        Node node = nodeObjectSelected.GetComponent<Node>();
        GetCostsUpgrade(node.turret);
        upgradeButtonText.text = ("Upgrade\n(" + upgradeCost +")");
        // Set the position of the buy menu to match the selected node
       upgradeMenu.transform.position = new Vector3(nodeObjectSelected.transform.position.x, buyMenu.transform.position.y, nodeObjectSelected.transform.position.z);
    }
    void HideUpgradeMenu()
    {
        MainCamera.SetActive(true);

        // Set the buy menu inactive
        upgradeMenu.SetActive(false);
    }

    public void setNodeObjectSelected(GameObject SelectedNode)
    {
        if (nodeObjectSelected == null)
        {
            nodeObjectSelected = SelectedNode;
            return;
        }
        Node node = nodeObjectSelected.GetComponent<Node>();
        node.menuShowing = false;
        nodeObjectSelected = SelectedNode;
    }
    public void BuildTurret()
    {
        GetCosts(turretToBuild);
        if(Currency.canaffoard(cost))
        {
            Node node = nodeObjectSelected.GetComponent<Node>();
            Vector3 SpawnPosition = nodeObjectSelected.transform.position;
            node.turret = Instantiate(turretToBuild, SpawnPosition, Quaternion.identity);
            Currency.amount -= cost;
            
            ToggleBuyMenu(true);
        }
           
      
    }

    public void UpgradeTurret()
    {
        
        Node node = nodeObjectSelected.GetComponent<Node>();
        GameObject turretToUpgrade = node.turret;
        IncreaseCosts(turretToUpgrade);
        GetCostsUpgrade(turretToUpgrade);
        if(Currency.canaffoard(upgradeCost))
        {
            TurretScript turret = turretToUpgrade.GetComponent<TurretScript>();
            turret.increaseLevel();

            Currency.amount -= upgradeCost;
            IncreaseCosts(node.turret);
            GetCostsUpgrade(turretToUpgrade);
            
            ToggleUpgradeMenu(true);
            Debug.Log("Upgraded");
        }
   
    }


    public void GetCosts(GameObject targetTurret)
    {
        TurretScript turret;
        if (targetTurret != null)
        {
            turret = targetTurret.GetComponent<TurretScript>();
        }
        else { return; }
        
        if (targetTurret == machineTurretPrefab)
        {
            Debug.Log("Gettinc costs");
            cost = MachineTurretCost;
            upgradeCost = MachineTurretUpgradeCost;
            upgradeIncrease = MachineTurretUpgradeIncrease;
        }
        else if (targetTurret == sniperTurretPrefab)
        {
            cost = sniperTurretCost;
            upgradeCost = sniperTurretUpgradeCost;
            upgradeIncrease = sniperTurretUpgradeIncrease;
        }
        else if (targetTurret == meleeTurretPrefab)
        {
            cost = meleeTurretCost;
            upgradeCost = meleeTurretUpgradeCost;
            upgradeIncrease = meleeTurretUpgradeIncrease;
        }
        else if (targetTurret == missileTurretPrefab)
        {
            cost = missileTurretCost;
            upgradeCost = missileTurretUpgradeCost;
            upgradeIncrease = missileTurretUpgradeIncrease;
        }
        else if (targetTurret == builderPrefab)
        {
            cost = builderCost;
            upgradeCost = builderUpgradeCost;
            upgradeIncrease = builderUpgradeIncrease;
        }
        else if (targetTurret == factoryPrefab)
        {
            cost = factoryCost;
            upgradeCost = factoryUpgradeCost;
            upgradeIncrease = factoryUpgradeIncrease;
        }
        Debug.Log("Increased cost");
    }
    public void GetCostsUpgrade(GameObject targetTurret)
    {
        TurretScript turret = targetTurret.GetComponent<TurretScript>();
        switch (turret.type)
        {
            case TurretScript.TurretType.Machine:
           upgradeCost = MachineTurretUpgradeCost;
            break;
            case TurretScript.TurretType.Sniper:
            upgradeCost = sniperTurretUpgradeCost;
            break;
            case TurretScript.TurretType.Melee:
            upgradeCost = meleeTurretUpgradeCost ;
            break;
            case TurretScript.TurretType.Missile:
            upgradeCost = missileTurretUpgradeCost ;
            break;
            case TurretScript.TurretType.Factory:
            upgradeCost = factoryUpgradeCost ;
            break;
            case TurretScript.TurretType.Builder:
            upgradeCost = builderUpgradeCost ;
            break;

        }
    }

    public void IncreaseCosts(GameObject targetTurret)
    {
        TurretScript turret = targetTurret.GetComponent<TurretScript>();
        switch(turret.type)
        {
            case TurretScript.TurretType.Machine:
                MachineTurretUpgradeCost = MachineTurretBaseUpgradeCost + (MachineTurretUpgradeIncrease* (turret.level-1));
           
            break;
            case TurretScript.TurretType.Sniper:
            sniperTurretUpgradeCost =sniperTurretBaseUpgradeCost * Mathf.Pow(sniperTurretUpgradeIncrease ,turret.level-1);
            break;
            case TurretScript.TurretType.Melee:
            meleeTurretUpgradeCost = meleeTurretBaseUpgradeCost + (meleeTurretUpgradeIncrease * (turret.level -1));
            break;
            case TurretScript.TurretType.Missile:
            missileTurretUpgradeCost = missileTurretBaseUpgradeCost * (missileTurretUpgradeIncrease * (turret.level -1));
            break;
            case TurretScript.TurretType.Factory:
            factoryUpgradeCost = factoryBaseUpgradeCost * Mathf.Pow(factoryUpgradeIncrease, turret.level - 1);
            break;
            case TurretScript.TurretType.Builder:
            builderUpgradeCost = builderBaseUpgradeCost * Mathf.Pow(builderUpgradeIncrease, turret.level - 1);
            break;

        }
    }
    public void takeHit()
    {
        PlayerHealth -= 10;
        HealthText.text = ("Health : " + PlayerHealth + "%");
        if(PlayerHealth <= 0)
        {
            Time.timeScale = 0f;
            UiElements[1].SetActive(false);
            UiElements[0].SetActive(true);

        }
    }
    //public void IncreaseCosts(GameObject targetTurret)
    //{

    //    if (targetTurret == null)
    //    {
    //        return;
    //    }
    //    else if (targetTurret == machineTurretPrefab)
    //    {
    //        MachineTurretUpgradeCost += MachineTurretUpgradeIncrease;
    //    }
    //    else if (targetTurret == sniperTurretPrefab)
    //    {
    //        sniperTurretUpgradeCost *= sniperTurretUpgradeIncrease;
    //    }
    //    else if (targetTurret == meleeTurretPrefab)
    //    {
    //        meleeTurretUpgradeCost += meleeTurretUpgradeIncrease;
    //    }
    //    else if (targetTurret == missileTurretPrefab)
    //    {
    //        missileTurretUpgradeCost += missileTurretUpgradeIncrease;
    //    }
    //    else if (targetTurret == builderPrefab)
    //    {
    //        builderUpgradeCost *= builderUpgradeIncrease;
    //    }
    //    else if (targetTurret == factoryPrefab)
    //    {
    //        factoryUpgradeCost *= factoryUpgradeIncrease;
    //    }
    //    // Add similar conditions for other turret types
    //}


}
