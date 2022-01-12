﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using static Satchel.FsmUtil;
using static CustomKnight.SkinManager;
using static Satchel.GameObjectUtils;

namespace CustomKnight
{
    internal class SpriteLoader : MonoBehaviour
    {
        private static GameObject loader;

        private static bool texRoutineRunning;
        private static Coroutine setTexRoutine;

        
        internal static bool LoadComplete { get; private set; }

        internal static Material _geoMat;
        internal static Material _knightMat;
        internal static Material _shadeMat;
        internal static Material _sprintMat;
        internal static Material _unnMat;
        internal static Material _cloakMat;
        internal static Material _shriekMat;
        internal static Material _wingsMat;
        internal static Material _quirrelMat;
        internal static Material _webbedMat;
        internal static Material _dreamMat;
        internal static Material _dnMat;
        internal static Material _hornetMat;
        internal static Material _birthMat;
        internal static Material _baldurMat;
        internal static Material _flukeMat;
        internal static Material _grimmMat;
        internal static Material _shieldMat;
        internal static Material _weaverMat;
        internal static Material _wombMat;
        internal static Material _wraithsMat;
        internal static Material _voidMat;
        internal static Material _vsMat;
        internal static Material _hudMat;
        internal static Material _LiquidMat;
        internal static void PullDefaultTextures()
        {
            if (!SkinManager.savedDefaultTextures)
            {
                SkinManager.Textures["Knight"].defaultTex = _knightMat.mainTexture as Texture2D;
                SkinManager.Textures["Sprint"].defaultTex = _sprintMat.mainTexture as Texture2D;
                SkinManager.Textures["Unn"].defaultTex = _unnMat.mainTexture as Texture2D;
                
                SkinManager.Textures["Liquid"].defaultTex = _LiquidMat.mainTexture as Texture2D;



                SkinManager.Textures["Shade"].defaultTex = _shadeMat.mainTexture as Texture2D;
                if(SkinManager.Skinables != null){
                    foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                        kvp.Value?.SaveTexture();
                    }
                }
                if (CustomKnight.GlobalSettings.Preloads)
                {
                    SkinManager.Textures["Cloak"].defaultTex = _cloakMat.mainTexture as Texture2D;
                    SkinManager.Textures["Shriek"].defaultTex = _shriekMat.mainTexture as Texture2D;
                    SkinManager.Textures["Wings"].defaultTex = _wingsMat.mainTexture as Texture2D;
                    SkinManager.Textures["Quirrel"].defaultTex = _quirrelMat.mainTexture as Texture2D;
                    SkinManager.Textures["Webbed"].defaultTex = _webbedMat.mainTexture as Texture2D;
                    SkinManager.Textures["DreamArrival"].defaultTex = _dreamMat.mainTexture as Texture2D;
                    SkinManager.Textures["Dreamnail"].defaultTex = _dreamMat.mainTexture as Texture2D;
                    SkinManager.Textures["Hornet"].defaultTex = _hornetMat.mainTexture as Texture2D;
                    SkinManager.Textures["Birthplace"].defaultTex = _birthMat.mainTexture as Texture2D;
                }

                SkinManager.Textures["Baldur"].defaultTex = _baldurMat.mainTexture as Texture2D;
                SkinManager.Textures["Fluke"].defaultTex = _flukeMat.mainTexture as Texture2D;
                SkinManager.Textures["Grimm"].defaultTex = _grimmMat.mainTexture as Texture2D;
                SkinManager.Textures["Shield"].defaultTex = _shieldMat.mainTexture as Texture2D;
                SkinManager.Textures["Weaver"].defaultTex = _weaverMat.mainTexture as Texture2D;
                SkinManager.Textures["Hatchling"].defaultTex = _wombMat.mainTexture as Texture2D;

                foreach (Transform child in HeroController.instance.gameObject.transform)
                {
                    if (child.name == "Spells")
                    {
                        foreach (Transform spellsChild in child)
                        {
                            if (spellsChild.name == "Scr Heads")
                            {
                                SkinManager.Textures["Wraiths"].defaultTex = spellsChild.gameObject
                                    .GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                            }
                            else if (spellsChild.name == "Scr Heads 2")
                            {
                                SkinManager.Textures["VoidSpells"].defaultTex = spellsChild.gameObject
                                    .GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                            }
                        }
                    }
                    else if (child.name == "Focus Effects")
                    {
                        foreach (Transform focusChild in child)
                        {
                            if (focusChild.name == "Heal Anim")
                            {
                                SkinManager.Textures["VS"].defaultTex = focusChild.gameObject
                                    .GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture as Texture2D;
                                break;
                            }
                        }
                    }
                }

                foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
                {
                    if (i.name == "Health 1")
                    {
                        SkinManager.Textures["Hud"].defaultTex =
                            i.GetCurrentSpriteDef().material.mainTexture as Texture2D;
                        break;
                    }
                }

                foreach (SpriteRenderer i in
                    GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (i.name == "Orb Full")
                    {
                        SkinManager.Textures["OrbFull"].defaultTex = i.sprite.texture;
                    }
                }

            }
        
            SkinManager.savedDefaultTextures = true;
        }
        internal static void UnloadAll()
        {    
            if (loader != null)
            {
                Destroy(loader);
            }

            if (HeroController.instance != null)
            {
                _knightMat.mainTexture = SkinManager.Textures["Knight"].defaultTex;
                _sprintMat.mainTexture = SkinManager.Textures["Sprint"].defaultTex;
                _unnMat.mainTexture = SkinManager.Textures["Unn"].defaultTex;

                _shadeMat.mainTexture = SkinManager.Textures["Shade"].defaultTex;

                foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                    kvp.Value.Reset();
                }

                if (CustomKnight.GlobalSettings.Preloads)
                {
                    _cloakMat.mainTexture = SkinManager.Textures["Cloak"].defaultTex;
                    _shriekMat.mainTexture = SkinManager.Textures["Shriek"].defaultTex;
                    _wingsMat.mainTexture = SkinManager.Textures["Wings"].defaultTex;
                    _quirrelMat.mainTexture = SkinManager.Textures["Quirrel"].defaultTex;
                    _webbedMat.mainTexture = SkinManager.Textures["Webbed"].defaultTex;
                    _dreamMat.mainTexture = SkinManager.Textures["DreamArrival"].defaultTex;
                    _dnMat.mainTexture = SkinManager.Textures["Dreamnail"].defaultTex;
                    _hornetMat.mainTexture = SkinManager.Textures["Hornet"].defaultTex;
                    _birthMat.mainTexture = SkinManager.Textures["Birthplace"].defaultTex;
                }
                
                _baldurMat.mainTexture = SkinManager.Textures["Baldur"].defaultTex;
                _flukeMat.mainTexture = SkinManager.Textures["Fluke"].defaultTex;
                _grimmMat.mainTexture = SkinManager.Textures["Grimm"].defaultTex;
                _shieldMat.mainTexture = SkinManager.Textures["Shield"].defaultTex;
                _weaverMat.mainTexture = SkinManager.Textures["Weaver"].defaultTex;
                _wombMat.mainTexture = SkinManager.Textures["Hatchling"].defaultTex;
            }
            
            if (texRoutineRunning && GameManager.instance != null)
            {
                GameManager.instance.StopCoroutine(setTexRoutine);
                texRoutineRunning = false;
            }

            DestroyObjects();
        }
        internal static void Load()
        {
            if (loader == null)
            {
                loader = new GameObject("Loader");
                loader.AddComponent<SpriteLoader>();
                DontDestroyOnLoad(loader);
            } else {
                SpriteLoader.LoadSprites();
            }
             
        }
        internal static void ModifyHeroTextures(SaveGameData data = null)
        {
            if (!texRoutineRunning)
            {
                setTexRoutine = GameManager.instance.StartCoroutine(SetHeroTex());
                texRoutineRunning = true;
            }
        }
        
        internal IEnumerator Start()
        {
            yield return new WaitWhile(
                () => HeroController.instance == null || GameManager.instance == null || GameManager.instance.gameMap == null
            );
            GameObject hc = HeroController.instance.gameObject;
            SceneManager sm = GameManager.instance.GetSceneManager().GetComponent<SceneManager>();

            tk2dSpriteAnimator anim = hc.GetComponent<tk2dSpriteAnimator>();
            _knightMat = anim.GetClipByName("Idle").frames[0].spriteCollection.spriteDefinitions[0].material;
            _sprintMat = anim.GetClipByName("Sprint").frames[0].spriteCollection.spriteDefinitions[0].material;
            _unnMat = anim.GetClipByName("Slug Up").frames[0].spriteCollection.spriteDefinitions[0].material;
            
            _LiquidMat = GameCameras.instance.hudCanvas.FindGameObjectInChildren("Liquid").GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            tk2dSpriteAnimator shadeAnim = sm.hollowShadeObject.GetComponent<tk2dSpriteAnimator>();
            _shadeMat = shadeAnim.GetClipByName("Idle").frames[0].spriteCollection.spriteDefinitions[0].material;

            
            _cloakMat = CustomKnight.GameObjects["Cloak"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _shriekMat = CustomKnight.GameObjects["Shriek"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _wingsMat = CustomKnight.GameObjects["Wings"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _quirrelMat = CustomKnight.GameObjects["Quirrel"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _webbedMat = CustomKnight.GameObjects["Webbed"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _dreamMat = CustomKnight.GameObjects["DreamArrival"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _dnMat = CustomKnight.GameObjects["Dreamnail"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _hornetMat = CustomKnight.GameObjects["Hornet"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            _birthMat = CustomKnight.GameObjects["Birthplace"].GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");

            GameObject baldur = charmEffects.FindGameObjectInChildren("Blocker Shield").FindGameObjectInChildren("Shell Anim");
            _baldurMat = baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            PlayMakerFSM poolFlukes = charmEffects.LocateMyFSM("Pool Flukes");
            GameObject fluke = poolFlukes.GetAction<CreateGameObjectPool>("Pool Normal", 0).prefab.Value;
            _flukeMat = fluke.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM spawnGrimmchild = charmEffects.LocateMyFSM("Spawn Grimmchild");
            GameObject grimm = spawnGrimmchild.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            _grimmMat = grimm.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM spawnOrbitShield = charmEffects.LocateMyFSM("Spawn Orbit Shield");
            GameObject orbitShield = spawnOrbitShield.GetAction<SpawnObjectFromGlobalPool>("Spawn", 2).gameObject.Value;
            GameObject shield = orbitShield.FindGameObjectInChildren("Shield");
            _shieldMat = shield.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM weaverlingControl = charmEffects.LocateMyFSM("Weaverling Control");
            GameObject weaver = weaverlingControl.GetAction<SpawnObjectFromGlobalPool>("Spawn", 0).gameObject.Value;
            _weaverMat = weaver.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            PlayMakerFSM hatchlingSpawn = charmEffects.LocateMyFSM("Hatchling Spawn");
            GameObject hatchling = hatchlingSpawn.GetAction<SpawnObjectFromGlobalPool>("Hatch", 2).gameObject.Value;
            _wombMat = hatchling.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;

            foreach (Transform child in hc.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            _wraithsMat = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            _voidMat = spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            _vsMat = focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
                            break;
                        }
                    }
                }
            }
            
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    _hudMat = i.GetCurrentSpriteDef().material;
                    break;
                }
            }

            LoadSprites();
        }

        private static void DestroyObjects()
        {
            foreach (KeyValuePair<string, CustomKnightTexture> pair in SkinManager.Textures)
            {
                CustomKnightTexture texture = pair.Value;
                if (texture.tex != null)
                {
                    Destroy(texture.tex);
                }
            }
            
            LoadComplete = false;
        }

        internal static void SetSkin(Dictionary<string, CustomKnightTexture> SkinMap){
            SkinManager.Textures = SkinMap;
            ModifyHeroTextures();
        }
        internal static void LoadSprites()
        {
            foreach (KeyValuePair<string,Skinable> kvp in SkinManager.Skinables)
            {
                kvp.Value.prepare();
            }
            if (SkinManager.SKIN_FOLDER == null)
            {
                SkinManager.SKIN_FOLDER = "Default";
            }
            LoadComplete = false;

            foreach (KeyValuePair<string, CustomKnightTexture> pair in SkinManager.Textures)
            {

                CustomKnightTexture texture = pair.Value;
                if(SkinManager.Skinables.TryGetValue(pair.Key, out var skinable)){
                    texture = skinable.ckTex;
                    Modding.Logger.Log($"{pair.Key} found");
                }
                string file = (SkinManager.SKINS_FOLDER + "/" + SkinManager.SKIN_FOLDER + "/" + texture.fileName).Replace("\\", "/");
                texture.missing = !File.Exists(file);
                
                if (!texture.missing)
                {
                    byte[] texBytes = File.ReadAllBytes(file);
                    if (texture.tex != null)
                    {
                        Destroy(texture.tex);
                    }
                    
                    texture.tex = new Texture2D(2, 2);
                    texture.tex.LoadImage(texBytes);
                    
                } else {
                    if (texture.tex != null)
                    {
                        Destroy(texture.tex);
                    }
                }    
            }

            SetSkin(SkinManager.Textures);
            LoadComplete = true;
        }

        private static IEnumerator SetHeroTex()
        {
            yield return new WaitWhile(() => !LoadComplete || HeroController.instance == null || CharmIconList.Instance == null);
            /*foreach(var psr in GameObject.FindObjectsOfType<ParticleSystemRenderer>())
            {
                psr.gameObject.LogWithChildren();
                DumpManager.debugDumpTex((Texture2D)psr.material.mainTexture,psr.name);
            }*/
            
            PullDefaultTextures();
            CustomKnight.swapManager.resetAllTextures();
            CustomKnight.swapManager.Swap(Path.Combine(SkinManager.SKINS_FOLDER,SKIN_FOLDER));

            var geoTexture = SkinManager.Textures["Geo"].currentTexture;
            if (geoTexture != null && _geoMat != null)
            {
               _geoMat.mainTexture = geoTexture;
            }

            _knightMat.mainTexture = SkinManager.Textures["Knight"].currentTexture;
           
            _sprintMat.mainTexture = SkinManager.Textures["Sprint"].currentTexture;
            _unnMat.mainTexture = SkinManager.Textures["Unn"].currentTexture;


            _LiquidMat.mainTexture = SkinManager.Textures["Liquid"].currentTexture;

            _shadeMat.mainTexture = SkinManager.Textures["Shade"].currentTexture;

            foreach(KeyValuePair<string,Skinable> kvp in SkinManager.Skinables){
                kvp.Value.Apply();
            }


            if (CustomKnight.GlobalSettings.Preloads)
            {
                _cloakMat.mainTexture = SkinManager.Textures["Cloak"].currentTexture;
                _shriekMat.mainTexture = SkinManager.Textures["Shriek"].currentTexture;
                _wingsMat.mainTexture = SkinManager.Textures["Wings"].currentTexture;
                _quirrelMat.mainTexture = SkinManager.Textures["Quirrel"].currentTexture;
                _webbedMat.mainTexture = SkinManager.Textures["Webbed"].currentTexture;
                _dreamMat.mainTexture = SkinManager.Textures["DreamArrival"].currentTexture;
                _dnMat.mainTexture = SkinManager.Textures["Dreamnail"].currentTexture;
                _hornetMat.mainTexture = SkinManager.Textures["Hornet"].currentTexture;
                _birthMat.mainTexture = SkinManager.Textures["Birthplace"].currentTexture;
            }

            _baldurMat.mainTexture = SkinManager.Textures["Baldur"].currentTexture;
            _flukeMat.mainTexture = SkinManager.Textures["Fluke"].currentTexture;
            _grimmMat.mainTexture = SkinManager.Textures["Grimm"].currentTexture;
            _shieldMat.mainTexture = SkinManager.Textures["Shield"].currentTexture;
            _weaverMat.mainTexture = SkinManager.Textures["Weaver"].currentTexture;
            _wombMat.mainTexture = SkinManager.Textures["Hatchling"].currentTexture;
            
            foreach (Transform child in HeroController.instance.gameObject.transform)
            {
                if (child.name == "Spells")
                {
                    foreach (Transform spellsChild in child)
                    {
                        if (spellsChild.name == "Scr Heads" || spellsChild.name == "Scr Base")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = SkinManager.Textures["Wraiths"].currentTexture;
                        }
                        else if (spellsChild.name == "Scr Heads 2" || spellsChild.name == "Scr Base 2")
                        {
                            spellsChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = SkinManager.Textures["VoidSpells"].currentTexture;
                        }
                    }
                }
                else if (child.name == "Focus Effects")
                {
                    foreach (Transform focusChild in child)
                    {
                        if (focusChild.name == "Heal Anim")
                        {
                            focusChild.gameObject.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = SkinManager.Textures["VS"].currentTexture;
                            break;
                        }
                    }
                }
            }
            
            
            foreach (tk2dSprite i in GameCameras.instance.hudCanvas.GetComponentsInChildren<tk2dSprite>())
            {
                if (i.name == "Health 1")
                {
                    i.GetCurrentSpriteDef().material.mainTexture = SkinManager.Textures["Hud"].currentTexture;
                    break;
                }
            }
            
            foreach (SpriteRenderer i in GameCameras.instance.hudCanvas.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (i.name == "Orb Full")
                {
                    Texture2D tex = SkinManager.Textures["OrbFull"].currentTexture;
                    i.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                }
                else if (i.name == "Pulse Sprite")
                {
                    if (i.gameObject != null)
                    {
                        Destroy(i.gameObject);
                    }
                }
            }

            texRoutineRunning = false;
        }

    }
}
