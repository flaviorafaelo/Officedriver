import "./millennium.dependencies"
import { Environment } from "lib/wts.platform"
import { IMillenniumFrame, IRoute, INotificationSource, INotificationItem } from 'lib/millennium.core'
import { loadNotifiers } from "./millennium.modules";
import { MillenniumBrowser } from "./browser/millennium.browser";
import "lib/bootstrap-datepicker.js";
import { Client } from "lib/wts.client";

//actual implementation 

if (!(window as any).millennium) (window as any).millennium = {};

export function bootstrapApp() {

	function handleSidebarMouseDown(e: Event) {
		if (!$.contains($('#sidebar')[0], e.target as any))
			hideSideBar();
	}

	function showSideBar() {
		$('#sidebar').css("display", "flex");
		setTimeout(() => {
			$('#sidebar').css("transform", "translateX(0)");
			$('#sidebar .search-box').focus();
		}, 50);
		$('.dimmer').addClass('dimmer-show');
		$(window).on("mousedown.sidebar touchstart.sidebar", handleSidebarMouseDown);
	}

	function hideSideBar(callback?: () => void) {
		$(window).off("mousedown.sidebar touchstart.sidebar");
		if ($('.dimmer').hasClass("dimmer-show")) {
			$('#sidebar').css({ "transform": "translateX(-102%)" });
			//setTimeout(1000, function () { $('#sidebar').hide() });
			$('.dimmer').removeClass('dimmer-show');
			if (callback)
				$('#sidebar').one("webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend", function (e) {
					if (callback && typeof callback === "function") callback()
					callback = undefined;
				});
		}
	}

	function adjustPreloader() {
		if ($('#preloader').is(":visible")) {
			$('#preloader').css('margin-left', (($(window).width() ?? 0) - ($('#preloader').width() ?? 0)) / 2);
			$('#preloader').css('margin-top', (($(window).height() ?? 0) - ($('#preloader').height() ?? 0)) / 2);
		}
	}

	function showPreloader(value: boolean) {
		if (value) {
			$('.dimmer').addClass('dimmer-show');
			$('#preloader').addClass("windows8");
			$('#preloader').show();
			adjustPreloader();
		}
		else {
			$('.dimmer').removeClass('dimmer-show');
			$('#preloader').hide();
			$('#preloader').removeClass("windows8");
		}
	}

	const notifiersList: (INotificationSource & { currentCount: number, currentList: INotificationItem[] })[] = [];

	function updateNotificationCount(notifier: INotificationSource & { currentCount?: number, currentList?: INotificationItem[] }, result: INotificationItem[]) {

		for (const item of result) {
			const found = notifier.currentList && notifier.currentList.find(e => e.key === item.key);
			if (!found) {
				millennium.browser.notify(item.description, "warning");
			}
		}

		notifier.currentList = result;
		notifier.currentCount = result.reduce((count, next) => count + 1, 0);

		const el = document.getElementById("notification_count");
		if (el) el.textContent = notifiersList.reduce((sum, next) => sum + next.currentCount, 0).toString();

		localStorage.setItem("last-notify-" + notifier.name, JSON.stringify(result));
	}

	async function startNotifications() {
		const notifiers = await loadNotifiers();
		for (const notifier of notifiers) {
			const instance = notifier.Notifier as INotificationSource & { currentCount: number, currentList: INotificationItem[] };

			const curListJSON = localStorage.getItem("last-notify-" + instance.name);
			if (curListJSON) instance.currentList = JSON.parse(curListJSON);
			notifiersList.push(instance);

			instance.subscribe(updateNotificationCount);
		}
	}

	let keepAliveInterval: any;
	function keepAlive() {
		Client.get("wtssystem/system/now");
	}

	let _routes: IRoute[];
	let _startupUrl: string;
	async function loginSuccess() {
		$("#login-div").blur();

		const navTree = $('.sidebar .menu-tree');

		navTree.empty();
		$("#licence_type").attr("disabled", "true");
		

		if (!millennium.browser.isEmbedded) {
			try {
				const text = await millennium.browser.getVersion();
				console.log("SERVER VERSION:" + text);
			} catch { }

			_routes = await millennium.browser.getRoutes();
			$('#sidebar').hide();
			navTree.treeview({
				data: _routes, levels: 1,
				onhoverColor: "rgba(255,255,255,0.3)",
				selectedBackColor: "var(--accent-color)",
				collapseIcon: "far fa-chevron-down",
				expandIcon: "far fa-chevron-right",
				nodeIcon: "",
				emptyIcon: "",
				onNodeSelected: nodeSelected
			});

			clearInterval(keepAliveInterval);
			const keepAliveTime = 60000 * 2 + Math.round(Math.random() * 60000);
			console.info("KEEP_ALIVE_TIME:", keepAliveTime);
			keepAliveInterval = setInterval(keepAlive, keepAliveTime);
		}

		try {
			millennium.browser.loginDone();
			if (_startupUrl) {

				let routeObj: any;
				let routeKey = _startupUrl;

				_startupUrl = "";
				while (routeKey.charAt(0) === "#" || routeKey.charAt(0) === "/")
					routeKey = routeKey.substring(1);

				if (!routeKey || routeKey.trim() === "") return;

				const paramsIdx = routeKey.indexOf("?");
				if (paramsIdx >= 0) {
					routeObj = JSON.parse(decodeURI(routeKey.substr(paramsIdx + 1)));
					routeKey = routeKey.substr(0, paramsIdx);
				}

				let curFrame = currentFrame();
				if (!curFrame) curFrame = millennium.browser.frame_create();
				await millennium.browser.load(curFrame, routeKey.toUpperCase(), routeObj, null);
			}
		}
		finally {
			showPreloader(false);
			hideLogin();
		}

		if (!millennium.browser.isEmbedded) {
			setTimeout(() => millennium.browser.notify("Login concluído", "success"), 0);
			startNotifications();
		}
	}

	function currentHost() {
		let host;
		if (localStorage) host = localStorage.getItem('defaultHost');
		if (host && host.indexOf(":") === -1) host = "https://" + host;
		return host || (window.location.protocol + "//" + window.location.hostname + ":" + window.location.port);
	}

	let currentLoginCallback: (host: string, userName: string, password: string) => void;

	async function defaultFinishLogin(host: string, userName: string, password: string) {
		if (localStorage) {
			localStorage.setItem('userName', userName);
			localStorage.setItem('defaultHost', host);
		}
		if (!millennium.browser.inLogin()) {
			showPreloader(true);
			try {
				// const licenceEl = $("#licence_type")[0] as HTMLSelectElement;
				let appName = "B5WIN32";
				const licenceName = "retag";
				// if (licenceName === "loja" || licenceName === "retag")//legacy


				localStorage.setItem("appname", appName);
				localStorage.setItem("licencename", licenceName);

				await millennium.browser.login(appName, currentHost(), userName, password)
				loginSuccess();
			}
			catch (e) {
				showPreloader(false);
				showLogin(true);
				millennium.browser.notify(e, "danger");
			}
		}
	}

	function notificationButtonClick() {
		for (const n of notifiersList) n.action();
	}

	function loginClick() {
		const result = currentLoginCallback($('#hostname').val() as string, ($("#username").val() as string).toUpperCase(), ($("#password").val() as string).toUpperCase());
	}

	function currentFrame() {
		return $("#header").find("ul").children(".active").data("mln-frame-instance") as IMillenniumFrame;
	}

	function newPageFromURL(url: string) {
		let curFrame = currentFrame();
		if (!curFrame) curFrame = millennium.browser.frame_create();
		millennium.browser.load(curFrame, undefined, url, null);
	}

	function nodeSelected(args: any, node: IRoute) {
		hideSideBar(() => { millennium.browser.load(currentFrame(), node.key, undefined) });
	}

	let loginDlg: any;// IMlnDialog;

	function hideLogin() {
		$("#login-div").hide();
		$("#content-frame").addClass("visible");
		//we need to remove for safety and because of a (very) wird chrome behaviour that
		//starts triggering keydown events for all over the place
		$("#password").remove();
		$(".home-hero-logo").removeClass("home-hero-logo-show");
		if (loginDlg) {
			//dialogManager.popDialog(loginDlg);
		}
	}

	async function loadLicenceTypes() {
		const licenceEl = $("#licence_type");
		if (licenceEl.children().length > 0) return;

		const curName = localStorage.getItem("licencename") || "OMNI";
		const result = await Client.get<any>("millenium:wtssystem.users.GetLicenceGroups", {}, { anonymous: true });
		for (const item of result.value) {
			const option = document.createElement("option");
			option.textContent = item.DESCRIPTION;
			option.value = item.NAME;
			option.setAttribute("data-type", item.TYPE);
			option.selected = curName === item.NAME;
			option.style.background = "var(--command-color)";
			option.style.color = "white";
			option.style.padding = "5px";
			licenceEl.append(option);
		}
	}

	function showLogin(blank = false, message = "", callback = defaultFinishLogin) {
		currentLoginCallback = callback;
		$("#password").remove();
		//creates the pwd element
		$("#pwd-group").append($(`<input autocomplete="off" class="light-placeholder" id="password" type="password" placeholder="Senha" style="border:none;background:none;width:100%;color:black"/>`));
		$("#login-message").text(message);
		$("#content-frame").removeClass("visible");
		$("#login_anim").addClass("show-fadein");
		$("#login-div").show();

		if ($("#username").val() !== "" || blank) {
			if (!blank) $("#username").attr("readonly", "true"); else $("#username").removeAttr("readonly");
			$("#password").focus();
		} else
			$("#username").focus();

		if (localStorage) {
			$("#hostname").val((blank) ? "" : localStorage.getItem("defaultHost") || "");
			$("#username").val((blank) ? "" : localStorage.getItem("userName") || "");
			$("#password").val("");
		}
		setTimeout(function () { $("#login_anim").removeClass("show-fadein") }, 1000);

		loginDlg = {
			title: "Login",
			close() {
				hideLogin();
			},
			getElement() {
				return $("#login-div")[0];
			}
		}

		loadLicenceTypes();
	}

	async function logout() {
		await millennium.browser.logout();
		window.location.reload();
	}

	function showUserOptions() {
		if ($("#usr-opt").css("display") == "none") {
			$("#usr-opt").css("display", "flex");
		} else {
			$("#usr-opt").css("display", "none");
		}
	}

	function filterRoutes(text: string): IRoute[] {

		function search(nodes: IRoute[]) {
			const searchResult = { nodes: [] as IRoute[], hasLeaf: false };
			for (const node of nodes) {
				if (node.nodes && node.nodes.length) {
					const nodeResult = search(node.nodes);
					searchResult.hasLeaf = searchResult.hasLeaf || nodeResult.hasLeaf;
					if (nodeResult.hasLeaf)
						searchResult.nodes.push({ ...node, nodes: nodeResult.nodes })
				}
				else if (node.text?.match(searchRegex)) {
					searchResult.hasLeaf = true;
					searchResult.nodes.push(node);
				}
			}
			return searchResult;
		}

		const searchRegex = new RegExp(text, "i");
		const result = search(_routes);
		if (result.hasLeaf)
			return result.nodes
		else
			return [];
	}

	let _searchTimeout: any;
	function handleSidebarSearch(e: Event) {
		clearTimeout(_searchTimeout);
		_searchTimeout = setTimeout(function () {
			const navTree = $('.sidebar .menu-tree');
			navTree.treeview("remove");
			const value = (e.target as HTMLInputElement).value;
			const filteredRoutes = filterRoutes(value);
			navTree.treeview({
				data: filteredRoutes, levels: value ? 10 : 1,
				onhoverColor: "rgba(255,255,255,0.3)",
				selectedBackColor: "var(--accent-color)",
				collapseIcon: "far fa-chevron-down",
				expandIcon: "far fa-chevron-right",
				nodeIcon: "",
				emptyIcon: "",
				onNodeSelected: nodeSelected
			});
		}, 50);
	}

	function setupUI() {

		if (!(millennium && millennium.browser && millennium.browser.isEmbedded)) {
			$('.mln-module-header').css("opacity", "1")
			$('#usr-img').attr("src", "https://www.gravatar.com/avatar?d=mm&s=28");
		}
		else {
			$('.mln-module-header').hide();
		}

		if (Environment.isMobile)
			window.document.body.classList.add("mobile");

		$('#menu-btn').click((e) => {
			showSideBar();
			e.stopPropagation();
		});

		if (location.protocol === "file:")
			$('.mln-host-input').show();

		$("#login").on("click", loginClick);

		$("#usr-img").on("click", showUserOptions);

		$("#logout").on("click", logout);

		$("#login-div").on("keydown", (e) => {
			if (e.keyCode === 13) $("#login").trigger("click")
		})

		$("#sidebar-close").on("click", () => hideSideBar());
		$("#sidebar-search").on("keyup", handleSidebarSearch);
		$("#sidebar-search").on("search", handleSidebarSearch);

		$("#notification_button").on("click", notificationButtonClick)

		window.onerror = (errorMsg) => {
			if (typeof errorMsg === "string") {
				errorMsg = errorMsg.replace("Uncaught ", "");
				if (errorMsg.substring(0, 5) === "show:") {
					millennium.browser.notify(errorMsg.substring(5), "danger");
				}
			}
		}

		document.addEventListener("touchstart", function () { }, false);
		document.addEventListener("contextmenu", function (e: Event) {
			e.preventDefault();
		}, false);

		const browserController = new MillenniumBrowser();

		browserController.onLoginDone.subscribe(hideLogin);

		browserController.onAuthFail.subscribe(async (options) => {
			if (options && options.blank) {
				showLogin(true, options.message, options.callback);
			}
			else if (!millennium.browser.isEmbedded)
				showLogin();
			else {
				try {
					const sessionId = await millennium.browser.getSessionId();
					await millennium.browser.joinSession(location.protocol + "//" + location.host, sessionId, {}, false);
				} catch (e) {
					alert(e);
				}
			}
		});

		//let the existing (native) implementation override ours    
		if (millennium.browser) {
			for (const hook in millennium.browser)
				if (millennium.browser.hasOwnProperty(hook))
					(browserController as any)[hook] = (millennium.browser as any)[hook]
		}

		millennium.browser = browserController;
	};

	async function startup(startupPage: string) {
		if (!millennium.browser.isEmbedded) {
			const versionInfoOmnichannel = await millennium.browser.getVersionInfoOmnichannel();
			const versionInfoMillennium = await millennium.browser.getVersionInfoMillennium();
			$(".versionOmnichannel").text(versionInfoOmnichannel);
			$(".versionMillennium").text(versionInfoMillennium);
			setTimeout(() => (document.querySelector(".home-hero-logo") as HTMLElement).classList.add("home-hero-logo-show"));
		}
		hideLogin();
		_startupUrl = startupPage;
		const rawSessionData = localStorage.getItem('sessionData');
		let sessionData = rawSessionData && JSON.parse(rawSessionData);
		if (!millennium.browser.isEmbedded) {
			if (!sessionData)
				showLogin()
			else {
				try {
					await millennium.browser.joinSession(currentHost(), sessionData.session, sessionData, true, localStorage.getItem("appname") || "");
					loginSuccess();
				} catch {
					showLogin()
				}
			}
		} else {
			try {
				const sessionId = await millennium.browser.getSessionId();
				await millennium.browser.joinSession(location.protocol + "//" + location.host, sessionId, {}, true);
				loginSuccess();
			} catch (e) {
				alert(e);
			}
		}
	}

	//wireup all events
	setupUI();
	if (!(window as any)['WebComponentsReady'])
		startup(location.hash);
	else window.addEventListener('WebComponentsReady', (e) => {
		startup(location.hash);
	})
}

window.addEventListener("load", () => bootstrapApp());
