import fakeRestDataProvider from "ra-data-fakerest";
import { DataProvider } from "react-admin";
import data from "./data.json";
import { Post, getPostsByStatus } from "./posts";

const baseDataProvider = fakeRestDataProvider(data, true);

export interface MyDataProvider extends DataProvider {
  updatePostStatus: (
    source: Post,
    destination: {
      status: Post["status"];
      index?: number; 
    }
  ) => Promise<void>;
}

export const dataProvider: MyDataProvider = {
  ...baseDataProvider,
  updatePostStatus: async (source, destination) => {
    const { data: unorderedPosts } = await dataProvider.getList<Post>("posts", {
      sort: { field: "index", order: "ASC" },
      pagination: { page: 1, perPage: 100 },
      filter: {},
    });

    const postsByStatus = getPostsByStatus(unorderedPosts);

    if (source.status === destination.status) {

      const columnPosts = postsByStatus[source.status];
      const destinationIndex = destination.index ?? columnPosts.length + 1;

      if (source.index > destinationIndex) {

        await Promise.all([
          ...columnPosts
            .filter(
              (post) =>
                post.index >= destinationIndex && post.index < source.index
            )
            .map((post) =>
              dataProvider.update("posts", {
                id: post.id,
                data: { index: post.index + 1 },
                previousData: post,
              })
            ),
          dataProvider.update("posts", {
            id: source.id,
            data: { index: destinationIndex },
            previousData: source,
          }),
        ]);
      } else {

        await Promise.all([
          ...columnPosts
            .filter(
              (post) =>
                post.index <= destinationIndex && post.index > source.index
            )
            .map((post) =>
              dataProvider.update("posts", {
                id: post.id,
                data: { index: post.index - 1 },
                previousData: post,
              })
            ),
          dataProvider.update("posts", {
            id: source.id,
            data: { index: destinationIndex },
            previousData: source,
          }),
        ]);
      }
    } else {

      const sourceColumn = postsByStatus[source.status];
      const destinationColumn = postsByStatus[destination.status];
      const destinationIndex =
        destination.index ?? destinationColumn.length + 1;

      await Promise.all([
        ...sourceColumn
          .filter((post) => post.index > source.index)
          .map((post) =>
            dataProvider.update("posts", {
              id: post.id,
              data: { index: post.index - 1 },
              previousData: post,
            })
          ),
        ...destinationColumn
          .filter((post) => post.index >= destinationIndex)
          .map((post) =>
            dataProvider.update("posts", {
              id: post.id,
              data: { index: post.index + 1 },
              previousData: post,
            })
          ),
        dataProvider.update("posts", {
          id: source.id,
          data: {
            index: destinationIndex,
            status: destination.status,
          },
          previousData: source,
        }),
      ]);
    }
  },
};
