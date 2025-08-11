using TMPro;
using UnityEngine;

/// <summary>
/// 현재 무기 이름과 탄약 UI 표시 프레젠터.
/// DIP: IWeaponProvider와 IWeapon 이벤트만 의존.
/// </summary>
[DisallowMultipleComponent]
public class WeaponHudPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private WeaponFireController weaponController; // 씬의 무기 컨트롤러

    private IWeaponProvider _provider;
    private IWeapon _current;

    void Awake()
    {
        if (!weaponController) weaponController = FindAnyObjectByType<WeaponFireController>();
        if (!weaponNameText) Debug.LogWarning("WeaponHudPresenter: weaponNameText is not assigned");
        if (!ammoText) Debug.LogWarning("WeaponHudPresenter: ammoText is not assigned");
    }

    void OnEnable()
    {
        if (!weaponController)
        {
            Debug.LogError("WeaponHudPresenter: WeaponFireController not found in scene.");
            return;
        }
        _provider = weaponController as IWeaponProvider;
        if (_provider == null)
        {
            Debug.LogError("WeaponHudPresenter: controller does not implement IWeaponProvider.");
            return;
        }

        weaponController.OnWeaponSwitched += OnWeaponSwitched;
        BindWeapon(_provider.CurrentWeapon);
    }

    void OnDisable()
    {
        weaponController.OnWeaponSwitched -= OnWeaponSwitched;
        UnbindWeaponEvents();
    }

    void OnWeaponSwitched(IWeapon weapon)
    {
        BindWeapon(weapon);
    }

    void BindWeapon(IWeapon weapon)
    {
        UnbindWeaponEvents();
        _current = weapon;
        if (_current != null)
        {
            _current.OnAmmoChanged += OnAmmoChanged;
            _current.OnReloadStarted += OnAmmoChanged;
            _current.OnReloadCompleted += OnAmmoChanged;
        }
        UpdateTexts();
    }

    void UnbindWeaponEvents()
    {
        if (_current != null)
        {
            _current.OnAmmoChanged -= OnAmmoChanged;
            _current.OnReloadStarted -= OnAmmoChanged;
            _current.OnReloadCompleted -= OnAmmoChanged;
        }
        _current = null;
    }

    void OnAmmoChanged(IWeapon _)
    {
        UpdateTexts();
    }

    void UpdateTexts()
    {
        if (weaponNameText)
        {
            weaponNameText.text = _current != null ? _current.WeaponName : "-";
        }
        if (ammoText)
        {
            if (_current == null)
            {
                ammoText.text = "-/-";
            }
            else if (_current.IsReloading)
            {
                ammoText.text = "Reloading...";
            }
            else
            {
                ammoText.text = $"{_current.CurrentAmmo}/{_current.MagazineSize}";
            }
        }
    }
}
