using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSO))]
public class WeaponSOEditor : Editor
{
    private SerializedProperty weaponPrefabProp;
    private SerializedProperty bulletPrefabProp;
    private SerializedProperty weaponNameProp;
    private SerializedProperty idleLocalPositionProp;
    private SerializedProperty bulletSpeedProp;
    private SerializedProperty impactForceProp;
    private SerializedProperty fireRateProp;
    private SerializedProperty kickForceProp;
    private SerializedProperty hasUnlimitedAmmoProp;
    private SerializedProperty maxAmmoProp;

    private void OnEnable()
    {
        weaponPrefabProp = serializedObject.FindProperty("weaponPrefab");
        bulletPrefabProp = serializedObject.FindProperty("bulletPrefab");
        weaponNameProp = serializedObject.FindProperty("weaponName");
        idleLocalPositionProp = serializedObject.FindProperty("idleLocalPosition");
        bulletSpeedProp = serializedObject.FindProperty("bulletSpeed");
        impactForceProp = serializedObject.FindProperty("impactForce");
        fireRateProp = serializedObject.FindProperty("fireRate");
        kickForceProp = serializedObject.FindProperty("kickForce");
        hasUnlimitedAmmoProp = serializedObject.FindProperty("hasUnlimitedAmmo");
        maxAmmoProp = serializedObject.FindProperty("maxAmmo");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponPrefabProp);
        EditorGUILayout.PropertyField(bulletPrefabProp);
        EditorGUILayout.PropertyField(weaponNameProp);
        EditorGUILayout.PropertyField(idleLocalPositionProp);
        EditorGUILayout.PropertyField(bulletSpeedProp);
        EditorGUILayout.PropertyField(impactForceProp);
        EditorGUILayout.PropertyField(fireRateProp);
        EditorGUILayout.PropertyField(kickForceProp);
        
        EditorGUILayout.PropertyField(hasUnlimitedAmmoProp);

        if (!hasUnlimitedAmmoProp.boolValue)
        {
            EditorGUILayout.PropertyField(maxAmmoProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
