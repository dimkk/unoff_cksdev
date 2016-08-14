/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.debug.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\MicrosoftAjax.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.Ribbon.debug.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.UI.debug.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.Core.debug.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.Dialog.debug.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\CUI.debug.js" />


Type.registerNamespace('$rootnamespace$.$fileinputname$');



$rootnamespace$.$fileinputname$.RibbonControlsPageComponent = function (webPartPageComponentId) {
    this._webPartPageComponentId = webPartPageComponentId;
    $rootnamespace$.$fileinputname$.RibbonControlsPageComponent.initializeBase(this);
}

$rootnamespace$.$fileinputname$.RibbonControlsPageComponent.initialize = function () {
    ExecuteOrDelayUntilScriptLoaded(Function.createDelegate(null, $rootnamespace$.$fileinputname$.RibbonControlsPageComponent.initializePageComponent), 'sp.ribbon.js');
}

$rootnamespace$.$fileinputname$.RibbonControlsPageComponent.initializePageComponent = function () {
    var ribbonPageManager = SP.Ribbon.PageManager.get_instance();
    if (null !== ribbonPageManager) {
        ribbonPageManager.addPageComponent($rootnamespace$.$fileinputname$.RibbonControlsPageComponent.instance);
    }
}

$rootnamespace$.$fileinputname$.RibbonControlsPageComponent.prototype = {
    init: function () {
        //TODO: add initialization functionality here
    },
    getFocusedCommands: function () {
        //TODO: add focused commands here
        return [
        '$rootnamespace$.$fileinputname$.ContextualGroupCommand',
        '$rootnamespace$.$fileinputname$.ContextualGroup.TabCommand',
        '$rootnamespace$.$fileinputname$.ContextualGroup.GroupCommand',
        '$rootnamespace$.$fileinputname$.ContextualGroup.Group.ButtonCommand'];

    },
    getGlobalCommands: function () {
        return [];
    },
    getId: function () {
        return this._webPartPageComponentId;
    },
    canHandleCommand: function (commandId) {
        // TODO: implement canHandleCommand
        var cmds = this.getFocusedCommands();
        for (var i = 0; i < cmds.length; i++) {
            if (cmds[i] == commandId) {
                return true;
            }
        }
        return false;

    },
    handleCommand: function (commandId, properties, sequence) {
        // TODO: implement the commands here
        switch (commandId) {
            case '$rootnamespace$.$fileinputname$.ContextualGroup.Group.ButtonCommand':
                alert('I\'m not implemented!');
                break;
            default:
                return false;
        }
    },
    isFocusable: function () {
        return true;
    }
}
$rootnamespace$.$fileinputname$.RibbonControlsPageComponent.registerClass('$rootnamespace$.$fileinputname$.RibbonControlsPageComponent', CUI.Page.PageComponent);
$rootnamespace$.$fileinputname$.pageComponentFactory = function(webPartPageComponentId, componentClass, jsFile, jsFilePath) {
    this.webPartPageComponentId = webPartPageComponentId;
    this.pageComponent = null;
    this.jsFile = jsFile;
    this.jsFilePath = jsFilePath;
    this.componentClass = componentClass;
    this.add = function () {
        if (typeof SP.Ribbon.PageManager.get_instance().getPageComponentById(webPartPageComponentId) == 'undefined') {
            pageComponent = new $rootnamespace$.$fileinputname$.RibbonControlsPageComponent(webPartPageComponentId);
            SP.Ribbon.PageManager.get_instance().addPageComponent(pageComponent);
        }
    }
}
$rootnamespace$.$fileinputname$.pageComponentFactory.prototype = {
    register: function () {
        SP.SOD.registerSod(this.jsFile, this.jsFilePath);
        SP.SOD.executeFunc(this.jsFile, this.componentClass, this.add);
    }
}
$rootnamespace$.$fileinputname$.create = function (webPartPageComponentId) {
    return new $rootnamespace$.$fileinputname$.pageComponentFactory(webPartPageComponentId, '$rootnamespace$.$fileinputname$.RibbonControlsPageComponent', '$fileinputname$/PageComponent.js', '/_layouts/$fileinputname$/PageComponent.js');
}
SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("$fileinputname$/PageComponent.js");

