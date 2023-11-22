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
        viewDataObj.container.contextmenu(function (event) {
            event.preventDefault();
            GraphCanvasPartial.openContextMenu(viewDataObj, event.clientY, event.clientX);
            return false;
        });

        // Save Button click
        viewDataObj.btnSave.on('click', function (e) { GraphCanvasPartial.onBtnSaveClick(viewDataObj); });

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

        let selectedEdges = viewDataObj.network.getSelectedEdges();
        if (selectedEdges != null && selectedEdges.length > 0) {
            var edgeId = selectedEdges[0];
            //var fromNodeIdx = -1;
            //var toNodeIdx = -1;

            //for (var i = 0; i < viewDataObj.nodes.length; i++) {
            //    var nodeIncidentToDeletedEdge =
            //        viewObj
            //            .network
            //            .body
            //            .nodes[i]
            //            .edges.filter(e => e.id == edgeId).length > 0;

            //    if (nodeIncidentToDeletedEdge && fromNodeIdx == -1)
            //        fromNodeIdx = viewObj.network.body.nodes[i].id;
            //    else if (nodeIncidentToDeletedEdge && toNodeIdx == -1)
            //        toNodeIdx = viewObj.network.body.nodes[i].id;
            //}

            //if (fromNodeIdx != -1 && toNodeIdx != -1) {
            //    //var edgeIdx =

            //    viewDataObj.edges.splice(, 1);
            //}

            var isDeleted = false;

            for (var i = 0; i < viewDataObj.edges.length; i++)
            {
                if (viewDataObj.edges[i].id == edgeId) {
                    isDeleted = true;
                    viewDataObj.edges.splice(i, 1);
                    viewDataObj.network.body.data.edges.remove(edgeId);
                    break;
                }
            }

            if (!isDeleted)
                alert("Error deleting edge!");
        }
    }

    this.onStartAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedStartNodes = viewDataObj.network.getSelectedNodes();
        if (selectedStartNodes != null && selectedStartNodes.length > 0) {
            viewDataObj.graphEdit.newEdgeStartNode = selectedStartNodes[0];
        }
    }

    this.onFinishAddingEdge = function (viewDataObj) {
        if (viewDataObj.network == null)
            return;

        let selectedEndNodes = viewDataObj.network.getSelectedNodes();
        if (selectedEndNodes != null && selectedEndNodes.length > 0) {
            viewDataObj.graphEdit.newEdgeEndNode = selectedEndNodes[0];

            if (viewDataObj.graphEdit.newEdgeStartNode != null) {
                var edgeId = viewDataObj.network.body.data.edges.add({ from: viewDataObj.graphEdit.newEdgeStartNode, to: viewDataObj.graphEdit.newEdgeEndNode });
                edgeId = edgeId[0];

                viewDataObj.edges.push({
                    from: viewDataObj.graphEdit.newEdgeStartNode,
                    to: viewDataObj.graphEdit.newEdgeEndNode,
                    id: edgeId
                });
            }

            viewDataObj.graphEdit.newEdgeStartNode = null;
            viewDataObj.graphEdit.newEdgeEndNode = null;
        }
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
            viewDataObj.container[0],
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
};