//----------------------------------------------
//       NGUI Infinite Pickers v1.53
// 		Copyright 2013 Gregorio Zanon
//----------------------------------------------

*** Important - How to update : Just like with NGUI, first create a new scene, then delete the NGUIPickers folder from the project view, then import NGUI Infinite Pickers from the asset store.
*** Note to JS users : NGUIPickers/Scripts should be moved to the Plugins folder.
*** Support : http://forum.unity3d.com/threads/189766-Released-NGUI-Infinite-Pickers-A-complete-picker-framework or by e-mail : basak.gregorio@gmail.com

v1.5 - 1.53 Release notes :

-Only compatible with NGUI v3.07+ 
-New CyclerClamper script to prevent the picker from looping: place on the Cycler GameObject and drag and drop the picker you wish to clamp to the observedPicker field.
Note that if you wish to center the first or last element in your collection, you will need to add numberOfCycledTransforms/2 empty items to your collection.

Fixes:
-Many small NGUI 3.07 compatbility fixes
-IPPickerLabelBase now sets UILabel.bitmapFont instead of font which is deprecated since NGUI 3.05
-ExpandablePicker is much more reliable
-Fixes related to the replacement of UIDraggablePanel by UIScrollView and of UIDragPanelContents by UIDragScrollView
-( 1.52 )very minor NGUI 3.08 compatibility fixes, fix in IPCycler.GetDeltaIndexFromScreenPos thanks to user loverainstm, thank you!

Note that when making a picker yourself, by adding picker scripts to the default picker for example, you will need to hit the ResetPicker button after setting the UIFont / UIAtlas for that picker.

Support thread, including tutorial videos, is linked on the product's Asset Store page. For reference : http://forum.unity3d.com/threads/189766-Soon-NGUI-Infinite-Pickers-A-complete-picker-framework

NGUI Infinite Picker's workflow is similar to NGUI's : a lot can be done in the inspector, without having to write any code. If you do need more control, take a look a the scripts in the Examples folder, 
they are well documented and should help you get a grasp of the framework.
 
Happy picking!

Gregzo

**********************************************************************

Previous Release Notes :

v1.1: 
-New : custom inspector for all pickers as well as for IPFrowardPickerEvents
-Fully WYSIWYG workflow!
-Expandable picker prefab
-Opacity and scaling of picker widgets in the new example scene

 *******************************************************
 
v1.2:

-2 new classes ectend NGUI's UITWeener class : TweenPanelClipRange and TweenPanelClipSoftness. They work like any other NGUI tweeners.
-PickerPopper modified to take advantage of this. Check the ExpandablePicker prefab in the demo scene.
-IPCycler now has a recenterSpringStrength field.
-IPSpringPanel script used in pickers instead of NGUI's SpringPanel - fixes an occasional bug with expandable pickers.
-Pickers handle interrupted recentering more graciously.

 *******************************************************
 
v1.3 : 

New functionalities :
-Added DragPickerSprite and RexeiveSpriteDrop scripts to the example's folder - see the new drag and drop scene.
-Added AutoScrollOnClick script : click any widget in a picker, it will get centered. Needs to be placed on the Background object ( the one with the collider ). 
No extra colliders involved, pickers will stay as responsive.
-Added restrictDragToPicker field in IPCycler component
-Added new picker type : IPTexturePicker. If shader is not set in the inspector, Unlit/Transparent Colored will be applied. Be careful : not all shaders work with UIPanel clipping!
-Pickers now automaticaly stop displaying widgets if there is no content to display
-It is now possible to directly insert or remove elements from IPSpricker.spriteNames, IPTextPicker.labelsText and IPTexturePicker.textures. Notify the picker afterwards with ResetPickerAtContentIndex 
( will snap the picker in place immediately ), or UpdateVirtualElementsCount ( changes will only show when next scrolling to the changed elements ).

New methods / fields / properties :
-Added OnDragExit callback in IPUserInteraction, access also with IPForwardPickerEvents
-Added SelectedIndex read only property to picker base class to allow retrieval of other indexes in picker
-Added AutoScrollBy method in IPCycler. Use to scroll more than 1 element at once ( max is number of cycled transforms / 2 ). See AutoScrollOnClick for typical use.
-Added GetDeltaIndexFromScreenPos method in IPCycler. Use to determine  where in the picker mouse pos or touch pos is ( 0 is center ). See AutoScrollOnClick for typical use.
-Added GetIndexFromScreenPos method in IPCycler.
-Added GetWidgetAtScreenPos method in all pickers. Example use in ScalePickerWidgetOnPress.cs.
-Added InsertElementAtIndex to IPSpritePicker and IPTextPicker
-Added optional resetPicker parameter to InsertElementAtIndex and RemoveElementAtIndex methods : InsertElementAtIndex ( string text, bool resetPicker = true ). InsertElement still resets by default.
Pass resetPicker = false if you need to add multiple elements at once, and then call ResetPicker().
-Added EnableWidgets ( bool enable ) to all pickers. 
-Added pickerName field to picker base class in order to differentiate when getting multiple picker components of the same type via GetComponents<T>()
-Exposed IPPickerBase.UpdateVirtualElementsCount () so that picker content can be dynamically updated without resetting the picker
-Exposed IPPickerBase.ResetWidgetsContent : call if you have changed a spriteName or labelsText and want to apply changes immediately if the element is visible in the picker.

Fixes :
-Fixed a serialization issue where prefabs would not serialize correctly before hitting reset
-Fixed divsion by zero error in IPNumberPicker
-Fixed how onCenterOnChildStarted delegate is handled in IPCycler : the event will now fire even when auto-scrolling.

Minor changes :
-Reorganized folders : example scenes, scripts, atlases and fonts are now in the Examples folder
-Sprite and label pickers can now have 0 elements
-IPNumberPicker now updates if min, max or step have changed, when calling ResetPickerAtValue.
-Fixed ResetPickerAtValue in IPNumberPicker : min, max and step can now be updated at runtime.
-Fixed inspector layouts for IPNumberPicker, IPForwardPickerEvents and IPCycler 

 *******************************************************
 
v1.4 :

-v1.4 only supports NGUI 3+. Please contact me by e-mail if you've accidentally updated and still use NGUI 2.7.
-Added widgetsDepth field to all pickers.
-Fixed bug in IPPickerBase.ResetWidgetsContent.
-IPPickerBase.UpdateWidget is now public.