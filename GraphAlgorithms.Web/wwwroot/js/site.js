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
        if (selectedEdges != null && selectedEdges.length > 0)
            viewDataObj.network.body.data.edges.remove(selectedEdges);
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
                viewDataObj.network.body.data.edges.add({ from: viewDataObj.graphEdit.newEdgeStartNode, to: viewDataObj.graphEdit.newEdgeEndNode });
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
};