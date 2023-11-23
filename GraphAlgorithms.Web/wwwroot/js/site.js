/*
    GRAPH CANVAS PARTIAL
*/
var GraphCanvasPartial = new function () {
    this.Initialize = function (viewDataObj) {
        $('body').on('click', function () { viewDataObj.contextMenu.removeClass('visible'); });

        // Context Menu Item click
        viewDataObj.contextMenu.find('.item').on('click', function (e) {
            GraphCanvasPartial.onContextMenuItemClick(viewDataObj, $(e.target));
        });

        // Opening Context Menu
        viewDataObj.graphContainer.contextmenu(function (event) {
            event.preventDefault();
            GraphCanvasPartial.openContextMenu(viewDataObj, event.clientY, event.clientX);
            return false;
        });

        // Save Button click
        viewDataObj.btnSave.on('click', function (e) { GraphCanvasPartial.onBtnSaveClick(viewDataObj); });

        // Full Screen click
        viewDataObj.btnFullScreen.on('click', function (e) { GraphCanvasPartial.onFullScreenBtnClick(viewDataObj); });

        // Initial network
        GraphCanvasPartial.createInitialNetwork(viewDataObj);
    }

    this.onContextMenuItemClick = function (viewDataObj, itemObj) {
        if (itemObj.hasClass('context-item-delete-edge'))
            GraphCanvasPartial.onDeleteEdge(viewDataObj);
        else if (itemObj.hasClass('context-item-start-adding-edge'))
            GraphCanvasPartial.onStartAddingEdge(viewDataObj);
        else if (itemObj.hasClass('context-item-finish-adding-edge'))
            GraphCanvasPartial.onFinishAddingEdge(viewDataObj);

        viewDataObj.contextMenu.removeClass('visible');
    }

    this.onDeleteEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        var selectedEdges = viewDataObj.network.getSelectedEdges();

        if (selectedEdges == null || selectedEdges.length == 0) {
            alert("Error: You must select edge.");
            return;
        }

        var edgeId = selectedEdges[0];
        var isDeleted = false;

        for (let i = 0; i < viewDataObj.edges.length; i++)
        {
            if (viewDataObj.edges[i].id == edgeId) {
                isDeleted = true;

                let fromNode = GraphCanvasPartial.getNodeByID(viewDataObj, viewDataObj.edges[i].from);
                let toNode = GraphCanvasPartial.getNodeByID(viewDataObj, viewDataObj.edges[i].to);

                GraphCanvasPartial.setStatus(viewDataObj, `Deleted edge ${fromNode.label} <-> ${toNode.label}`);

                viewDataObj.edges.splice(i, 1);
                viewDataObj.network.body.data.edges.remove(edgeId);
                break;
            }
        }

        if (!isDeleted)
            alert("Error deleting edge!");
    }

    this.onStartAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedStartNodes = viewDataObj.network.getSelectedNodes();
        if (selectedStartNodes != null && selectedStartNodes.length > 0) {
            viewDataObj.graphEdit.newEdgeStartNode = selectedStartNodes[0];

            var node = GraphCanvasPartial.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeStartNode);

            GraphCanvasPartial.setStatus(viewDataObj, `Adding edge with starting node ${node.label}`);
        }
    }

    this.onFinishAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedEndNodes = viewDataObj.network.getSelectedNodes();
        if (selectedEndNodes == null || selectedEndNodes.length == 0) {
            alert("Error: You must select node.");
            return;
        }

        viewDataObj.graphEdit.newEdgeEndNode = selectedEndNodes[0];

        if (viewDataObj.graphEdit.newEdgeStartNode != null) {
            var edgeId = viewDataObj.network.body.data.edges.add({ from: viewDataObj.graphEdit.newEdgeStartNode, to: viewDataObj.graphEdit.newEdgeEndNode });
            edgeId = edgeId[0];

            viewDataObj.edges.push({
                from: viewDataObj.graphEdit.newEdgeStartNode,
                to: viewDataObj.graphEdit.newEdgeEndNode,
                id: edgeId
            });

            let fromNode = GraphCanvasPartial.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeStartNode);
            let toNode = GraphCanvasPartial.getNodeByID(viewDataObj, viewDataObj.graphEdit.newEdgeEndNode);

            GraphCanvasPartial.setStatus(viewDataObj, `Added edge ${fromNode.label} <-> ${toNode.label}`);
        }

        viewDataObj.graphEdit.newEdgeStartNode = null;
        viewDataObj.graphEdit.newEdgeEndNode = null;
    }

    this.openContextMenu = function (viewDataObj, posY, posX) {
        viewDataObj.contextMenu.removeClass("visible");
        viewDataObj.contextMenu.css({ top: posY, left: posX });

        //setTimeout(() => {
            viewDataObj.contextMenu.addClass("visible");
        //}, 10);
    }

    this.createInitialNetwork = function (viewDataObj) {
        viewDataObj.network = new vis.Network(
            viewDataObj.graphContainer[0],
            {
                nodes: new vis.DataSet(viewDataObj.nodes),
                edges: new vis.DataSet(viewDataObj.edges)
            },
            {}
        );
    }

    this.onBtnSaveClick = function (viewDataObj) {
        $.post("/GraphDrawing/Store", {
            nodes: viewDataObj.nodes,
            edges: viewDataObj.edges,
            score: 0
        }).done(function (data) {
            alert("Saved");
        });
    }

    this.onFullScreenBtnClick = function (viewDataObj) {
        if (
            document.fullscreenElement ||
            document.webkitFullscreenElement ||
            document.mozFullScreenElement ||
            document.msFullscreenElement
        )
        {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitExitFullscreen) {
                document.webkitExitFullscreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            }
        }
        else {
            let container = viewDataObj.canvasContainer.get(0);

            if (container.requestFullscreen) {
                container.requestFullscreen();
            } else if (container.mozRequestFullScreen) {
                container.mozRequestFullScreen();
            } else if (container.webkitRequestFullscreen) {
                container.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            } else if (container.msRequestFullscreen) {
                container.msRequestFullscreen();
            }
        }
    }

    this.setStatus = function (viewDataObj, status) {
        viewDataObj.statusMsg.html(`Status: ${status}`);
    }

    this.getNodeByID = function (viewDataObj, id) {
        var nodes = viewDataObj.nodes.filter(n => n.id == id);

        if (nodes.length > 0)
            return nodes[0];
        else
            return null;
    }
};