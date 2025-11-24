using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Weapon Visuals")]
    [Tooltip("The GameObject that holds the iPod sprite.")]
    public GameObject ipodWeaponVisual;

    [Tooltip("The GameObject that holds the Gun sprite.")]
    public GameObject gunWeaponVisual;

    [Header("Switch Settings")]
    [Tooltip("The key to press to switch weapons.")]
    public KeyCode switchKey = KeyCode.R;

    // An enum to keep track of the current weapon state. It's cleaner than using a boolean.
    private enum ActiveWeapon { iPod, Gun }
    private ActiveWeapon currentWeapon;

    void Start()
    {
        // Let's start with the iPod as the default weapon.
        // We set the iPod to be active and the Gun to be inactive.
        currentWeapon = ActiveWeapon.iPod;
        ipodWeaponVisual.SetActive(true);
        gunWeaponVisual.SetActive(false);

        Debug.Log("Weapon system initialized. Current weapon: iPod");
    }

    void Update()
    {
        // Listen for the switch key press.
        if (Input.GetKeyDown(switchKey))
        {
            // Call the function to switch to the next weapon.
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        // Check which weapon is currently active and switch to the other one.
        if (currentWeapon == ActiveWeapon.iPod)
        {
            // Switch to Gun
            currentWeapon = ActiveWeapon.Gun;
            ipodWeaponVisual.SetActive(false);
            gunWeaponVisual.SetActive(true);
            Debug.Log("Switched to: Gun");
        }
        else // If the current weapon is the Gun
        {
            // Switch to iPod
            currentWeapon = ActiveWeapon.iPod;
            ipodWeaponVisual.SetActive(true);
            gunWeaponVisual.SetActive(false);
            Debug.Log("Switched to: iPod");
        }
    }
}
