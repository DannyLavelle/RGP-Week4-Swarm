using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
   


    BuildManager buildManager;
    private void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void PurchaseMachineTurret()
    {
        buildManager.setTurretToBuild(buildManager.machineTurretPrefab);
        buildManager.BuildTurret();          
    }
    public void PurchaseMeleeTurret()
    {
        buildManager.setTurretToBuild(buildManager.meleeTurretPrefab);
        buildManager.BuildTurret();
    }
    public void PurchaseSniperTurret()
    {
        buildManager.setTurretToBuild(buildManager.sniperTurretPrefab);
        buildManager.BuildTurret();
    }
    public void PurchaseMissileTurret()
    {
        buildManager.setTurretToBuild(buildManager.missileTurretPrefab);
        buildManager.BuildTurret();
    }
    public void PurchaseBuilder()
    {
        buildManager.setTurretToBuild(buildManager.builderPrefab);
        buildManager.BuildTurret();
    }
    public void PurchaseFactory()
    {
        buildManager.setTurretToBuild(buildManager.factoryPrefab);
        buildManager.BuildTurret();
    }
    public void UpgradeTurret()
    {
        buildManager.UpgradeTurret();
    }
}
